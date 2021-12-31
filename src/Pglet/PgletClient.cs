using Pglet.Protocol;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace Pglet
{
    public class PgletClient
    {
        [DllImport("libc", SetLastError = true)]
        private static extern int chmod(string pathname, int mode);

        // user permissions
        const int S_IRUSR = 0x100;
        const int S_IWUSR = 0x80;
        const int S_IXUSR = 0x40;

        // group permission
        const int S_IRGRP = 0x20;
        const int S_IWGRP = 0x10;
        const int S_IXGRP = 0x8;

        // other permissions
        const int S_IROTH = 0x4;
        const int S_IWOTH = 0x2;
        const int S_IXOTH = 0x1;

        public const string HOSTED_SERVICE_URL = "https://app.pglet.io";
        public const string DEFAULT_SERVER_PORT = "5000";
        public const string ZERO_SESSION = "0";

        private PgletClient() { }

        static PgletClient()
        {
            // subscribe to application exit/unload events
            Console.CancelKeyPress += delegate
            {
                OnExit();
            };

            AppDomain.CurrentDomain.ProcessExit += (sender, e) =>
            {
                OnExit();
            };
        }

        public static async Task<Page> ConnectPage(string pageName = null, bool web = false,
            string serverUrl = null, string token = null, bool noWindow = false, string permissions = null,
            Func<Connection, string, string, string, Page> createPage = null, CancellationToken? cancellationToken = null)
        {
            var ct = cancellationToken.HasValue ? cancellationToken.Value : CancellationToken.None;
            var conn = await ConnectInternal(pageName, false, web, serverUrl, token, permissions, noWindow, null, createPage, ct);

            Page page = createPage != null ? createPage(conn, conn.PageUrl, conn.PageName, ZERO_SESSION) : new Page(conn, conn.PageUrl, conn.PageName, ZERO_SESSION);
            await page.LoadPageDetails();
            conn.Sessions[ZERO_SESSION] = page;
            return page;
        }

        public static async Task ServeApp(Func<Page, Task> sessionHandler, string pageName = null, bool web = false,
            string serverUrl = null, string token = null, bool noWindow = false, string permissions = null,
            Func<Connection, string, string, string, Page> createPage = null, Action<string> pageCreated = null, CancellationToken? cancellationToken = null)
        {
            var ct = cancellationToken.HasValue ? cancellationToken.Value : CancellationToken.None;
            var conn = await ConnectInternal(pageName, true, web, serverUrl, token, permissions, noWindow, sessionHandler, createPage, ct);

            pageCreated?.Invoke(conn.PageUrl);

            var semaphore = new SemaphoreSlim(0);
            using CancellationTokenRegistration ctr = ct.Register(() =>
            {
                semaphore.Release();
            });
            await semaphore.WaitAsync();
            conn.Close();
        }

        private static async Task<Connection> ConnectInternal(string pageName, bool isApp,
            bool web, string serverUrl, string token, string permissions, bool noWindow,
            Func<Page, Task> sessionHandler, Func<Connection, string, string, string, Page> createPage,
            CancellationToken cancellationToken)
        {
            if (String.IsNullOrEmpty(serverUrl))
            {
                if (web)
                {
                    serverUrl = HOSTED_SERVICE_URL;
                }
                else
                {
                    var serverPort = Environment.GetEnvironmentVariable("PGLET_SERVER_PORT");
                    serverUrl = "http://localhost:" + (string.IsNullOrEmpty(serverPort) ? DEFAULT_SERVER_PORT : serverPort);
                }
            }

            var wsUrl = GetWebSocketUrl(serverUrl);
            var ws = new ReconnectingWebSocket(wsUrl);
            var conn = new Connection(ws);
            conn.OnEvent = async (payload) =>
            {
                //Console.WriteLine("Event received: " + JsonUtility.Serialize(payload));
                if (conn.Sessions.TryGetValue(payload.SessionID, out Page page))
                {
                    await page.OnEvent(new Event
                    {
                        Target = payload.EventTarget,
                        Name = payload.EventName,
                        Data = payload.EventData
                    });

                    if (payload.EventTarget == "page" && payload.EventName == "close")
                    {
                        //Console.WriteLine("Session is closing: " + payload.SessionID);
                        conn.Sessions.TryRemove(payload.SessionID, out Page _);
                    }
                }
            };

            if (sessionHandler != null)
            {
                conn.OnSessionCreated = async (payload) =>
                {
                    Trace.TraceInformation("Session created: " + JsonUtility.Serialize(payload));
                    Page page = createPage != null ? createPage(conn, conn.PageUrl, conn.PageName, payload.SessionID) : new Page(conn, conn.PageUrl, conn.PageName, payload.SessionID);
                    await page.LoadPageDetails();
                    conn.Sessions[payload.SessionID] = page;

                    var h = sessionHandler(page).ContinueWith(async t =>
                    {
                        if (t.IsFaulted)
                        {
                            await page.ErrorAsync("There was an error while processing your request: " + (t.Exception as AggregateException).InnerException.Message);
                        }
                    });
                };
            }

            ws.OnFailedConnect = () =>
            {
                if (wsUrl.Host == "localhost")
                {
                    StartPgletServer();
                }
                return Task.CompletedTask;
            };
            ws.OnReconnected = async () =>
            {
                await conn.RegisterHostClient(pageName, isApp, token, permissions, cancellationToken);
            };

            await ws.Connect(cancellationToken);

            var resp = await conn.RegisterHostClient(pageName, isApp, token, permissions, cancellationToken);
            conn.PageName = resp.PageName;
            conn.PageUrl = GetPageUrl(serverUrl, conn.PageName).ToString();

            if (!noWindow)
            {
                OpenBrowser(conn.PageUrl);
            }

            return conn;
        }

        private static Uri GetPageUrl(string serverUrl, string pageName)
        {
            var pageUri = new UriBuilder(serverUrl ?? HOSTED_SERVICE_URL);
            pageUri.Path = "/" + pageName;
            return pageUri.Uri;
        }

        private static Uri GetWebSocketUrl(string serverUrl)
        {
            var wssUri = new UriBuilder(serverUrl ?? HOSTED_SERVICE_URL);
            wssUri.Scheme = wssUri.Scheme == "https" ? "wss" : "ws";
            wssUri.Path = "/ws";
            return wssUri.Uri;
        }

        private static void StartPgletServer()
        {
            Trace.TraceInformation("Starting Pglet Server in local mode");
            var platform = "win";
            if (RuntimeInfo.IsLinux)
            {
                platform = "linux";
            }
            else if (RuntimeInfo.IsMac)
            {
                platform = "osx";
            }
            var pgletPath = Path.Combine(GetApplicationDirectory(), "runtimes", $"{platform}-{RuntimeInfo.Architecture}", RuntimeInfo.IsWindows ? "pglet-server.exe" : "pglet");
            
            if (!File.Exists(pgletPath))
            {
                // override for local development
                pgletPath = RuntimeInfo.IsWindows ? "pglet.exe" : "pglet";
            }
            else if (RuntimeInfo.IsLinux || RuntimeInfo.IsMac)
            {
                // set chmod
                const int _0755 = S_IRUSR | S_IXUSR | S_IWUSR | S_IRGRP | S_IXGRP | S_IROTH | S_IXOTH;
                chmod(pgletPath, (int)_0755);
            }

            Trace.TraceInformation("Pglet executable path: {0}", pgletPath);

            var psi = new ProcessStartInfo
            {
                FileName = pgletPath,
                Arguments = "server --background",
                CreateNoWindow = true,
                UseShellExecute = false
            };

            Process.Start(psi);
        }

        private static void OpenBrowser(string url)
        {
            string procVer = "/proc/version";
            bool wsl = (RuntimeInfo.IsLinux && File.Exists(procVer) && File.ReadAllText(procVer).ToLowerInvariant().Contains("microsoft"));
            if (RuntimeInfo.IsWindows || wsl)
            {
                Process.Start("explorer.exe", url);
            }
            else if (RuntimeInfo.IsMac)
            {
                Process.Start("open", url);
            }
        }

        private static string GetApplicationDirectory()
        {
            return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        }

        private static void OnExit()
        {
            Trace.TraceInformation("Exiting from program...");
            Connection.CloseAllConnections();
        }
    }
}

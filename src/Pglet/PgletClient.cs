using Pglet.Protocol;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Pglet
{
    public class PgletClient
    {
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
            var conn = await ConnectInternal(pageName, false, web, serverUrl, token, permissions, noWindow, ct);

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
            var conn = await ConnectInternal(pageName, true, web, serverUrl, token, permissions, noWindow, ct);

            pageCreated?.Invoke(conn.PageUrl);

            // new session handler
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

            var semaphore = new SemaphoreSlim(0);
            using CancellationTokenRegistration ctr = ct.Register(() =>
            {
                semaphore.Release();
            });
            await semaphore.WaitAsync();
            conn.Close();
        }

        private static async Task<Connection> ConnectInternal(string pageName, bool isApp, bool web, string serverUrl, string token, string permissions, bool noWindow, CancellationToken cancellationToken)
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
            conn.OnEvent = (payload) =>
            {
                //Console.WriteLine("Event received: " + JsonUtility.Serialize(payload));
                if (conn.Sessions.TryGetValue(payload.SessionID, out Page page))
                {
                    page.OnEvent(new Event
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
                return Task.CompletedTask;
            };

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

            Process.Start(pgletPath, "server --background");
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

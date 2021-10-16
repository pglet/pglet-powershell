using Pglet.Protocol;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Pglet
{
    public class PgletClient : IDisposable
    {
        public const string HOSTED_SERVICE_URL = "https://console.pglet.io";
        public const string ZERO_SESSION = "0";

        ReconnectingWebSocket _ws;
        Connection _conn;
        string _hostClientId;
        string _pageName;
        string _pageUrl;
        ConcurrentDictionary<string, Page> _sessions = new ConcurrentDictionary<string, Page>();

        public async Task<Page> ConnectPage(string pageName = null,
            string serverUrl = null, string token = null, bool noWindow = false, string permissions = null,
            Func<Connection, string, string, string, Page> createPage = null, CancellationToken? cancellationToken = null)
        {
            var ct = cancellationToken.HasValue ? cancellationToken.Value : CancellationToken.None;
            await ConnectInternal(pageName, false, serverUrl, token, permissions, noWindow, ct);

            Page page = createPage != null ? createPage(_conn, _pageUrl, _pageName, ZERO_SESSION) : new Page(_conn, _pageUrl, _pageName, ZERO_SESSION);
            await page.LoadHash();
            _sessions[ZERO_SESSION] = page;
            return page;
        }

        public async Task ServeApp(Func<Page, Task> sessionHandler, string pageName = null,
            string serverUrl = null, string token = null, bool noWindow = false, string permissions = null,
            Func<Connection, string, string, string, Page> createPage = null, Action<string> pageCreated = null, CancellationToken? cancellationToken = null)
        {
            var ct = cancellationToken.HasValue ? cancellationToken.Value : CancellationToken.None;
            await ConnectInternal(pageName, true, serverUrl, token, permissions, noWindow, ct);

            pageCreated?.Invoke(_pageUrl);

            // new session handler
            _conn.OnSessionCreated = async (payload) =>
            {
                Console.WriteLine("Session created: " + JsonUtility.Serialize(payload));
                Page page = createPage != null ? createPage(_conn, _pageUrl, _pageName, payload.SessionID) : new Page(_conn, _pageUrl, _pageName, payload.SessionID);
                await page.LoadHash();
                _sessions[payload.SessionID] = page;

                var h = sessionHandler(page).ContinueWith(async t =>
                {
                    if (t.IsFaulted)
                    {
                        await page.ErrorAsync("There was an error while processing your request: " + (t.Exception as AggregateException).InnerException.Message);
                    }
                });
            };

            var semaphore = new SemaphoreSlim(0);
            using CancellationTokenRegistration ctr = ct.Register(() => {
                semaphore.Release();
            });
            await semaphore.WaitAsync();
        }

        private async Task ConnectInternal(string pageName, bool isApp, string serverUrl, string token, string permissions, bool noWindow, CancellationToken cancellationToken)
        {
            _ws = new ReconnectingWebSocket(GetWebSocketUrl(serverUrl));
            await _ws.Connect(cancellationToken);
            _conn = new Connection(_ws);
            _conn.OnEvent = OnPageEvent;

            _ws.OnReconnected = async () =>
            {
                await _conn.RegisterHostClient(_hostClientId, pageName, isApp, token, permissions, cancellationToken);
            };

            var resp = await _conn.RegisterHostClient(_hostClientId, pageName, isApp, token, permissions, cancellationToken);
            _hostClientId = resp.HostClientID;
            _pageName = resp.PageName;
            _pageUrl = GetPageUrl(serverUrl, _pageName).ToString();

            if (!noWindow)
            {
                OpenBrowser(_pageUrl);
            }
        }

        private Task OnPageEvent(PageEventPayload payload)
        {
            //Console.WriteLine("Event received: " + JsonUtility.Serialize(payload));
            if (_sessions.TryGetValue(payload.SessionID, out Page page))
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
                    _sessions.TryRemove(payload.SessionID, out Page _);
                }
            }
            return Task.CompletedTask;
        }

        private Uri GetPageUrl(string serverUrl, string pageName)
        {
            var pageUri = new UriBuilder(serverUrl ?? HOSTED_SERVICE_URL);
            pageUri.Path = "/" + pageName;
            return pageUri.Uri;
        }

        private Uri GetWebSocketUrl(string serverUrl)
        {
            var wssUri = new UriBuilder(serverUrl ?? HOSTED_SERVICE_URL);
            wssUri.Scheme = wssUri.Scheme == "https" ? "wss" : "ws";
            wssUri.Path = "/ws";
            return wssUri.Uri;
        }

        private void OpenBrowser(string url)
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

        public void Close()
        {
            if (_conn != null)
            {
                _conn.Close();
            }
        }

        public void Dispose()
        {
            Close();
        }
    }
}

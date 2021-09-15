using Pglet.Protocol;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Pglet
{
    public class PgletClient2
    {
        public const string HOSTED_SERVICE_URL = "https://console.pglet.io";

        ReconnectingWebSocket _ws;
        ConnectionWS _conn;
        string _hostClientId;
        string _pageName;
        string _pageUrl;
        ConcurrentDictionary<string, Page> _sessions = new ConcurrentDictionary<string, Page>();

        public async Task<Page> ConnectPage(string pageName = null,
            string serverUrl = null, string token = null, bool noWindow = false, string permissions = null,
            Func<Connection, string, Page> createPage = null, CancellationToken? cancellationToken = null)
        {
            await ConnectInternal(pageName, false, serverUrl, token, permissions, cancellationToken.HasValue ? cancellationToken.Value : CancellationToken.None);

            var commands = new List<Command>
            {
                new Command
                {
                    Name = "clean",
                    Values = new List<string> { "page" }
                }
            };
            var resp1 = await _conn.SendCommands(_pageName, "0", commands, CancellationToken.None);
            commands.Clear();

            for (int i = 0; i < 10; i++)
            {
                commands.Add(new Command
                {
                    Name = "add",
                    Values = new List<string> { "button" },
                    Attrs = new Dictionary<string, string>
                    {
                        { "text", $"Button {i}" },
                        { "primary", "true" },
                    }
                });
            }
            var resp2 = await _conn.SendCommands(_pageName, "0", commands, CancellationToken.None);
            return null;
        }

        public async Task ServeApp(Func<Page, Task> sessionHandler, string pageName = null,
            string serverUrl = null, string token = null, bool noWindow = false, string permissions = null,
            Func<Connection, string, Page> createPage = null, Action<string> pageCreated = null, CancellationToken? cancellationToken = null)
        {
            await ConnectInternal(pageName, true, serverUrl, token, permissions, cancellationToken.HasValue ? cancellationToken.Value : CancellationToken.None);

            pageCreated?.Invoke(_pageUrl);

            var tcs = new TaskCompletionSource<bool>();
            if (cancellationToken.HasValue)
            {
                using CancellationTokenRegistration ctr = cancellationToken.Value.Register(() => {
                    tcs.SetCanceled();
                });
            }

            await tcs.Task;
        }

        private async Task ConnectInternal(string pageName, bool isApp, string serverUrl, string token, string permissions, CancellationToken cancellationToken)
        {
            _ws = new ReconnectingWebSocket(GetWebSocketUrl(serverUrl ?? HOSTED_SERVICE_URL));
            await _ws.Connect(cancellationToken);
            _conn = new ConnectionWS(_ws);

            _conn.OnEvent = (payload) =>
            {
                Console.WriteLine("Event received: " + JsonUtility.Serialize(payload));
                return Task.CompletedTask;
            };

            if (isApp)
            {
                _conn.OnSessionCreated = (payload) =>
                {
                    Console.WriteLine("Session created: " + JsonUtility.Serialize(payload));
                    return Task.CompletedTask;
                };
            }

            var resp = await _conn.RegisterHostClient(pageName, isApp, token, permissions, cancellationToken);
            _hostClientId = resp.HostClientID;
            _pageName = resp.PageName;
            _pageUrl = GetPageUrl(serverUrl, _pageName).ToString();
        }

        //private Task OnSessionCreated(PageSessionCreatedPayload payload)
        //{

        //}

        private Uri GetPageUrl(string serverUrl, string pageName)
        {
            var pageUri = new UriBuilder(serverUrl);
            pageUri.Path = "/" + pageName;
            return pageUri.Uri;
        }

        private Uri GetWebSocketUrl(string serverUrl)
        {
            var wssUri = new UriBuilder(serverUrl);
            wssUri.Scheme = wssUri.Scheme == "https" ? "wss" : "ws";
            wssUri.Path = "/ws";
            return wssUri.Uri;
        }
    }
}

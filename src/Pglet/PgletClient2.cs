using Pglet.Protocol;
using System;
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

        public async Task<Page> ConnectPage(string pageName = null,
            string server = null, string token = null, bool noWindow = false, string permissions = null,
            Func<Connection, string, Page> createPage = null, CancellationToken? cancellationToken = null)
        {
            _ws = new ReconnectingWebSocket(GetWebSocketUrl(server ?? HOSTED_SERVICE_URL));
            await _ws.Connect(cancellationToken.HasValue ? cancellationToken.Value : CancellationToken.None);
            var conn = new ConnectionWS(_ws);

            conn.OnEvent = (payload) =>
            {
                Console.WriteLine("Event received: " + JsonUtility.Serialize(payload));
                return Task.CompletedTask;
            };

            var resp = await conn.RegisterHostClient(pageName, false, token, permissions, CancellationToken.None);

            var commands = new List<Command>
            {
                new Command
                {
                    Name = "clean",
                    Values = new List<string> { "page" }
                }
            };
            var resp1 = await conn.SendCommands(resp.PageName, resp.SessionID, commands, CancellationToken.None);
            commands.Clear();

            for (int i = 0; i < 500; i++)
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
            var resp2 = await conn.SendCommands(resp.PageName, resp.SessionID, commands, CancellationToken.None);
            return null;
        }

        public async Task ServeApp(Func<Page, Task> sessionHandler, string name = null,
            string server = null, string token = null, bool noWindow = false, string permissions = null,
            Func<Connection, string, Page> createPage = null, Action<string> pageCreated = null, CancellationToken? cancellationToken = null)
        {
            _ws = new ReconnectingWebSocket(GetWebSocketUrl(server ?? HOSTED_SERVICE_URL));
            await _ws.Connect(cancellationToken.HasValue ? cancellationToken.Value : CancellationToken.None);
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

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

        public async Task<Page> ConnectPage(string name = null,
            string server = null, string token = null, bool noWindow = false, string permissions = null,
            Func<Connection, string, Page> createPage = null, CancellationToken? cancellationToken = null)
        {
            _ws = new ReconnectingWebSocket(GetWebSocketUrl(server ?? HOSTED_SERVICE_URL));
            await _ws.Connect(cancellationToken.HasValue ? cancellationToken.Value : CancellationToken.None);
            var conn = new ConnectionWS(_ws);
            var resp = await conn.RegisterHostClient(name, false, token, permissions);
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

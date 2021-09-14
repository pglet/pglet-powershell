using System;
using System.Collections.Generic;
using System.Text;
using System.Net.WebSockets;
using System.Threading.Tasks;
using System.Threading;
using Pglet.Protocol;
using System.Collections.Concurrent;
using Newtonsoft.Json.Linq;

namespace Pglet
{
    public class ConnectionWS
    {
        ReconnectingWebSocket _ws;
        ConcurrentDictionary<string, TaskCompletionSource<JObject>> _wsCallbacks = new ConcurrentDictionary<string, TaskCompletionSource<JObject>>();

        public ConnectionWS(ReconnectingWebSocket ws)
        {
            _ws = ws;
            _ws.OnMessage = OnMessage;
        }

        private Task OnMessage(byte[] message)
        {
            var j = Encoding.UTF8.GetString(message);
            var m = JsonUtility.Deserialize<Message>(j);

            if (!String.IsNullOrEmpty(m.Id))
            {
                // it's a callback
                if (_wsCallbacks.TryRemove(m.Id, out TaskCompletionSource<JObject> tcs))
                {
                    tcs.SetResult(m.Payload as JObject);
                }
            }
            else
            {
                // TODO
                // other events
            }

            return Task.CompletedTask;
        }

        public async Task<RegisterHostClientResponsePayload> RegisterHostClient(string pageName, bool isApp, string authToken, string permissions)
        {
            // TODO
            // send hostClientID on WS reconnect

            var payload = new RegisterHostClientRequestPayload
            {
                PageName = String.IsNullOrEmpty(pageName) ? "*" : pageName,
                IsApp = isApp,
                AuthToken = authToken,
                Permissions = permissions
            };

            var respPayload = await SendMessageWithResult("registerHostClient", payload, CancellationToken.None);
            return JsonUtility.Deserialize<RegisterHostClientResponsePayload>(respPayload);
        }

        public async Task<JObject> SendMessageWithResult(string actionName, object payload, CancellationToken cancellationToken)
        {
            // send request
            var msg = new Message
            {
                Id = Guid.NewGuid().ToString("N"),
                Action = actionName,
                Payload = payload
            };

            var j = JsonUtility.Serialize(msg);
            var jb = Encoding.UTF8.GetBytes(j);
            await _ws.SendMessage(jb);

            // register TSC for response
            var tcs = new TaskCompletionSource<JObject>();
            _wsCallbacks.TryAdd(msg.Id, tcs);

            using CancellationTokenRegistration ctr = cancellationToken.Register(() => {
                if (_wsCallbacks.TryRemove(msg.Id, out TaskCompletionSource<JObject> tcs))
                {
                    tcs.SetCanceled();
                }
            });

            return await tcs.Task;
        }
    }
}

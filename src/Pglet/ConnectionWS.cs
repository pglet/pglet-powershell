﻿using System;
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
        Func<PageEventPayload, Task> _onEvent;
        Func<PageSessionCreatedPayload, Task> _onSessionCreated;

        public Func<PageEventPayload, Task> OnEvent
        {
            set { _onEvent = value; }
        }

        public Func<PageSessionCreatedPayload, Task> OnSessionCreated
        {
            set { _onSessionCreated = value; }
        }

        public ConnectionWS(ReconnectingWebSocket ws)
        {
            _ws = ws;
            _ws.OnMessage = OnMessage;
        }

        private async Task OnMessage(byte[] message)
        {
            var j = Encoding.UTF8.GetString(message);
            var m = JsonUtility.Deserialize<Message>(j);

            if (m.Payload == null)
            {
                throw new Exception("Invalid message received by a WebSocket");
            }

            if (!String.IsNullOrEmpty(m.Id))
            {
                // it's a callback
                if (_wsCallbacks.TryRemove(m.Id, out TaskCompletionSource<JObject> tcs))
                {
                    tcs.SetResult(m.Payload as JObject);
                }
            }
            else if (m.Action == Actions.PageEventToHost && _onEvent != null)
            {
                // page event
                await _onEvent(JsonUtility.Deserialize<PageEventPayload>(m.Payload as JObject));
            }
            else if (m.Action == Actions.SessionCreated && _onSessionCreated != null)
            {
                // new session started
                await _onSessionCreated(JsonUtility.Deserialize<PageSessionCreatedPayload>(m.Payload as JObject));
            }
            else
            {
                // something else
                // TODO - throw?
                Console.WriteLine(m.Payload);
            }
        }

        public async Task<RegisterHostClientResponsePayload> RegisterHostClient(string pageName, bool isApp, string authToken, string permissions, CancellationToken cancellationToken)
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

            var respPayload = await SendMessageWithResult(Actions.RegisterHostClient, payload, cancellationToken);
            return JsonUtility.Deserialize<RegisterHostClientResponsePayload>(respPayload);
        }

        public async Task<PageCommandResponsePayload> SendCommand(string pageName, string sessionId, Command command, CancellationToken cancellationToken)
        {
            var payload = new PageCommandRequestPayload
            {
                PageName = pageName,
                SessionID = sessionId,
                Command = command
            };

            var respPayload = await SendMessageWithResult(Actions.PageCommandFromHost, payload, cancellationToken);
            return JsonUtility.Deserialize<PageCommandResponsePayload>(respPayload);
        }

        public async Task<PageCommandsBatchResponsePayload> SendCommands(string pageName, string sessionId, List<Command> commands, CancellationToken cancellationToken)
        {
            var payload = new PageCommandsBatchRequestPayload
            {
                PageName = pageName,
                SessionID = sessionId,
                Commands = commands
            };

            var respPayload = await SendMessageWithResult(Actions.PageCommandsBatchFromHost, payload, cancellationToken);
            return JsonUtility.Deserialize<PageCommandsBatchResponsePayload>(respPayload);
        }

        private Task<JObject> SendMessage(string actionName, object payload, CancellationToken cancellationToken)
        {
            return SendMessageInternal(null, actionName, payload, cancellationToken);
        }

        private Task<JObject> SendMessageWithResult(string actionName, object payload, CancellationToken cancellationToken)
        {
            return SendMessageInternal(Guid.NewGuid().ToString("N"), actionName, payload, cancellationToken);
        }

        private async Task<JObject> SendMessageInternal(string messageId, string actionName, object payload, CancellationToken cancellationToken)
        {
            // send request
            var msg = new Message
            {
                Id = messageId,
                Action = actionName,
                Payload = payload
            };

            var j = JsonUtility.Serialize(msg);
            var jb = Encoding.UTF8.GetBytes(j);
            await _ws.SendMessage(jb, cancellationToken);

            if (messageId != null)
            {
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
            else
            {
                // shoot and forget
                return null;
            }
        }
    }
}
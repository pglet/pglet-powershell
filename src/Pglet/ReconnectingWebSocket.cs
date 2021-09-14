using System;
using System.Collections.Generic;
using System.IO;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Pglet
{
    public class ReconnectingWebSocket
    {
        private const int RECEIVE_BUFFER_SIZE = 1024;
        private const int SEND_BUFFER_SIZE = 1024;

        ClientWebSocket _ws;
        Uri _uri;
        Func<byte[], Task> _onMessage;
        CancellationToken _cancellationToken;

        public ReconnectingWebSocket(Uri uri, Func<byte[], Task> onMessage)
        {
            _uri = uri;
            _onMessage = onMessage;
        }

        public async Task Connect(CancellationToken cancellationToken)
        {
            _cancellationToken = cancellationToken;
            _ws = new ClientWebSocket();
            await _ws.ConnectAsync(_uri, _cancellationToken);
            StartReadLoop();
        }

        private void StartReadLoop()
        {
            _ = Task.Factory.StartNew(
                function: () => ReadLoop(),
                cancellationToken: CancellationToken.None,
                creationOptions: TaskCreationOptions.LongRunning,
                scheduler: TaskScheduler.Default
            );
        }

        private async Task ReadLoop()
        {
            var buffer = new byte[RECEIVE_BUFFER_SIZE];

            try
            {
                while (_ws.State == WebSocketState.Open)
                {
                    var ms = new MemoryStream();

                    WebSocketReceiveResult result;
                    do
                    {
                        result = await _ws.ReceiveAsync(new ArraySegment<byte>(buffer), _cancellationToken);

                        if (result.MessageType != WebSocketMessageType.Close)
                        {
                            await ms.WriteAsync(buffer, 0, result.Count);
                        }
                        else
                        {
                            await Close();
                        }

                    } while (!result.EndOfMessage);

                    if (_onMessage != null)
                    {
                        await _onMessage(ms.ToArray());
                    }
                }
            }
            catch
            {
                // TODO
                // re-connection logic
                throw;
            }
        }

        public async Task SendMessage(byte[] message)
        {
            // TODO
            // block until connected

            try
            {
                var messagesCount = (int)Math.Ceiling((double)message.Length / SEND_BUFFER_SIZE);
                for (var i = 0; i < messagesCount; i++)
                {
                    var offset = (SEND_BUFFER_SIZE * i);
                    var count = SEND_BUFFER_SIZE;
                    var lastMessage = ((i + 1) == messagesCount);

                    if ((count * (i + 1)) > message.Length)
                    {
                        count = message.Length - offset;
                    }

                    await _ws.SendAsync(new ArraySegment<byte>(message, offset, count), WebSocketMessageType.Binary, lastMessage, _cancellationToken);
                }
            }
            catch
            {
                // TODO
                // re-connection logic
                throw;
            }
        }

        public async Task Close()
        {
            var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));
            await _ws.CloseAsync(WebSocketCloseStatus.NormalClosure, "", cts.Token);
            _ws.Dispose();

            // call _onDisconnected
        }
    }
}

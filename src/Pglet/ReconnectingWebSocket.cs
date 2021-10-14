using System;
using System.IO;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Pglet
{
    public class ReconnectingWebSocket
    {
        private const int RECEIVE_BUFFER_SIZE = 1024;
        private const int SEND_BUFFER_SIZE = 1024;

        private readonly Channel<byte[]> _sendQueue = Channel.CreateBounded<byte[]>(10);

        ClientWebSocket _ws;
        Uri _uri;
        Func<byte[], Task> _onMessage;
        CancellationToken _cancellationToken;

        public Func<byte[], Task> OnMessage
        {
            set { _onMessage = value; }
        }

        public ReconnectingWebSocket(Uri uri)
        {
            _uri = uri;
        }

        public async Task Connect(CancellationToken cancellationToken)
        {
            _cancellationToken = cancellationToken;
            _ws = new ClientWebSocket();
            await _ws.ConnectAsync(_uri, _cancellationToken);
            StartReadWriteLoops();
        }

        private void StartReadWriteLoops()
        {
            _ = Task.Factory.StartNew(
                function: () => ReadLoop(),
                cancellationToken: _cancellationToken,
                creationOptions: TaskCreationOptions.LongRunning,
                scheduler: TaskScheduler.Default
            );

            _ = Task.Factory.StartNew(
                function: () => WriteLoop(),
                cancellationToken: _cancellationToken,
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

                    //Console.WriteLine("WS before read");

                    WebSocketReceiveResult result;
                    do
                    {
                        result = await _ws.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                        //Console.WriteLine("WS read");

                        if (result.MessageType != WebSocketMessageType.Close)
                        {
                            await ms.WriteAsync(buffer, 0, result.Count);
                        }
                        else
                        {
                            // connection closed
                            return;
                        }

                    } while (!result.EndOfMessage);

                    if (_onMessage != null)
                    {
                        var t = _onMessage(ms.ToArray());
                    }
                }
            }
            catch (TaskCanceledException)
            {
                throw;
            }
            catch (Exception ex)
            {
                // TODO
                // re-connection logic
                throw ex;
            }
        }

        public async Task SendMessage(byte[] message, CancellationToken cancellationToken)
        {
            await _sendQueue.Writer.WriteAsync(message, cancellationToken);
        }

        public async Task WriteLoop()
        {
            while(true)
            {
                var message = await _sendQueue.Reader.ReadAsync(_cancellationToken);

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
                catch(OperationCanceledException)
                {
                    throw;
                }
                catch
                {
                    // TODO
                    // re-connection logic
                    throw;
                }
            }
        }

        public async Task Close()
        {
            var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));
            await _ws.CloseAsync(WebSocketCloseStatus.NormalClosure, "", cts.Token).ConfigureAwait(false);
            _ws.Dispose();

            // call _onDisconnected
        }
    }
}

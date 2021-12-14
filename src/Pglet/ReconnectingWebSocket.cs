using System;
using System.Diagnostics;
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
        private const int RECONNECT_DELAY_MS = 1000;
        private const int MAX_RECONNECT_DELAY_MS = 60000;

        private const int REMOTE_CONNECT_TIMEOUT_MS = 5000;
        private const int REMOTE_CONNECT_ATTEMPTS = 1;

        private const int LOCAL_CONNECT_TIMEOUT_MS = 200;
        private const int LOCAL_CONNECT_ATTEMPTS = 50;

        private readonly Channel<byte[]> _sendQueue = Channel.CreateBounded<byte[]>(10);

        ClientWebSocket _ws;
        Uri _uri;
        Func<byte[], Task> _onMessage;
        Func<Task> _onFailedConnect;
        Func<Task> _onReconnected;

        CancellationToken _cancellationToken;
        CancellationTokenSource _loopsCts;
        CancellationTokenSource _linkedCts;
        CancellationTokenRegistration _reconnectCtr;

        public Func<byte[], Task> OnMessage
        {
            set { _onMessage = value; }
        }

        public Func<Task> OnFailedConnect
        {
            set { _onFailedConnect = value; }
        }

        public Func<Task> OnReconnected
        {
            set { _onReconnected = value; }
        }

        public ReconnectingWebSocket(Uri uri)
        {
            _uri = uri;
        }

        public Task Connect(CancellationToken cancellationToken)
        {
            Trace.TraceInformation("ReconnectingWebSocket: Connect()");
            _cancellationToken = cancellationToken;
            return ConnectInternal();
        }

        private async Task ConnectInternal()
        {
            Trace.TraceInformation("ReconnectingWebSocket: ConnectInternal()");
            bool isLocal = _uri.Host == "localhost";

            bool failedConnectCalled = false;
            for (int i = 1; i < (isLocal ? LOCAL_CONNECT_ATTEMPTS : REMOTE_CONNECT_ATTEMPTS) + 1; i++)
            {
                using (CancellationTokenSource timeout = new CancellationTokenSource(isLocal ? LOCAL_CONNECT_TIMEOUT_MS : REMOTE_CONNECT_TIMEOUT_MS))
                {
                    using (var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(_cancellationToken, timeout.Token))
                    {
                        try
                        {
                            _ws = new ClientWebSocket();
                            await _ws.ConnectAsync(_uri, linkedCts.Token);
                            Trace.TraceInformation("Connected!");
                            break;
                        }
                        catch (Exception ex)
                        {
                            Trace.TraceInformation($"Connect attempt #{i} failed: {ex.Message}");
                            _ws.Dispose();
                            if (_onFailedConnect != null && !failedConnectCalled)
                            {
                                _ = _onFailedConnect();
                                failedConnectCalled = true;
                            }
                        }
                    }
                }
            }

            StartReadWriteLoops();
        }

        private void StartReadWriteLoops()
        {
            Trace.TraceInformation("ReconnectingWebSocket: StartReadWriteLoops()");

            if (_loopsCts != null)
            {
                _loopsCts.Dispose();
                _reconnectCtr.Dispose();
            }
            _loopsCts = new CancellationTokenSource();
            _reconnectCtr = _loopsCts.Token.Register(async () =>
            {
                Trace.TraceInformation("ReconnectingWebSocket: Reconnecting...");
                _ws.Abort();

                ExponentialBackoff backoff = new(RECONNECT_DELAY_MS, MAX_RECONNECT_DELAY_MS);
                while (true)
                {
                    try
                    {
                        await ConnectInternal();

                        if (_onReconnected != null)
                        {
                            _ = _onReconnected();
                        }

                        return;
                    }
                    catch (Exception ex)
                    {
                        Trace.TraceInformation("ReconnectingWebSocket: Error reconnecting: {0}", ex.Message);
                        await backoff.Delay();
                    }
                }
            });

            if (_linkedCts != null)
            {
                _linkedCts.Dispose();
            }
            _linkedCts = CancellationTokenSource.CreateLinkedTokenSource(_loopsCts.Token, _cancellationToken);

            _ = Task.Factory.StartNew(
                function: () => ReadLoop(),
                cancellationToken: _linkedCts.Token,
                creationOptions: TaskCreationOptions.LongRunning,
                scheduler: TaskScheduler.Default
            );

            _ = Task.Factory.StartNew(
                function: () => WriteLoop(),
                cancellationToken: _linkedCts.Token,
                creationOptions: TaskCreationOptions.LongRunning,
                scheduler: TaskScheduler.Default
            );
        }

        private async Task ReadLoop()
        {
            Trace.TraceInformation("ReconnectingWebSocket: ReadLoop()");
            var buffer = new byte[RECEIVE_BUFFER_SIZE];

            try
            {
                try
                {
                    while (_ws.State == WebSocketState.Open)
                    {
                        var ms = new MemoryStream();

                        //Console.WriteLine("WS before read");

                        WebSocketReceiveResult result;
                        do
                        {
                            result = await _ws.ReceiveAsync(new ArraySegment<byte>(buffer), _loopsCts.Token);

                            //Console.WriteLine("WS read");

                            if (result.MessageType != WebSocketMessageType.Close)
                            {
                                await ms.WriteAsync(buffer, 0, result.Count, _linkedCts.Token);
                            }
                            else
                            {
                                // connection closed
                                Trace.TraceInformation("ReconnectingWebSocket: Server connection gracefully closed while receiving message");
                                return;
                            }

                        } while (!result.EndOfMessage);

                        if (_onMessage != null)
                        {
                            _ = _onMessage(ms.ToArray());
                        }
                    }
                }
                catch (TaskCanceledException)
                {
                    return;
                }
                catch (Exception ex)
                {
                    Trace.TraceError("ReconnectingWebSocket: Error receiving message: {0}", ex.Message);
                    _loopsCts.Cancel();
                    return;
                }
            }
            finally
            {
                Trace.TraceInformation("ReconnectingWebSocket: Exiting read loop");
            }
        }

        public async Task SendMessage(byte[] message, CancellationToken cancellationToken)
        {
            await _sendQueue.Writer.WriteAsync(message, cancellationToken);
        }

        public async Task WriteLoop()
        {
            Trace.TraceInformation("ReconnectingWebSocket: WriteLoop()");
            try
            {
                while (true)
                {
                    try
                    {
                        var message = await _sendQueue.Reader.ReadAsync(_linkedCts.Token);

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

                            await _ws.SendAsync(new ArraySegment<byte>(message, offset, count), WebSocketMessageType.Binary, lastMessage, _linkedCts.Token);
                        }
                    }
                    catch (OperationCanceledException)
                    {
                        return;
                    }
                    catch (Exception ex)
                    {
                        Trace.TraceError("ReconnectingWebSocket: Error sending message: {0}", ex.Message);
                        _loopsCts.Cancel();
                        return;
                    }
                }
            }
            finally
            {
                Trace.TraceInformation("ReconnectingWebSocket: Exiting write loop");
            }
        }

        public async Task CloseAsync()
        {
            Trace.TraceInformation("ReconnectingWebSocket: CloseAsync()");

            if (_ws != null)
            {
                using (var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10)))
                {
                    await _ws.CloseAsync(WebSocketCloseStatus.NormalClosure, "", cts.Token);
                }

                _ws.Dispose();
            }

            if (_loopsCts != null)
            {
                _loopsCts.Dispose();
                _reconnectCtr.Dispose();
            }

            if (_linkedCts != null)
            {
                _linkedCts.Dispose();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Net.WebSockets;
using System.Threading.Tasks;
using System.Threading;

namespace Pglet
{
    public class ConnectionWS
    {
        public async Task Connect()
        {
            ClientWebSocket cws = new ClientWebSocket();
            await cws.ConnectAsync(new Uri("ws://localhost:5000/ws"), CancellationToken.None);

            var message = "{ \"id\": \"\", \"action\": \"registerHostClient\", \"payload\": { \"pageName\": \"page-1\" } }";
            byte[] sendBytes = Encoding.UTF8.GetBytes(message);
            var sendBuffer = new ArraySegment<byte>(sendBytes);
            await cws.SendAsync(sendBuffer, WebSocketMessageType.Text, endOfMessage: true, cancellationToken: CancellationToken.None);
            await cws.CloseAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None);
        }
    }
}

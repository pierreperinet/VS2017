using System;
using System.Diagnostics;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.WebSockets;

namespace MankalaHTML
{
    public class WebSocketHandlerB : IHttpHandler
    {
        private static WebSocket _webSocket;
        static readonly UTF8Encoding Encoder = new UTF8Encoding();

        public void ProcessRequest(HttpContext context)
        {
            // context.Response.ContentType = "text/plain";
            // context.Response.Write("Hello World");

            // Checks if the query is WebSocket request. 
            if (context.IsWebSocketRequest)
            {
                context.AcceptWebSocketRequest(WebSocketRequestHandler);
            }
        }

        public bool IsReusable => false;

        public async Task WebSocketRequestHandler(AspNetWebSocketContext webSocketContext)
        {
            _webSocket = webSocketContext.WebSocket;
            const int maxMessageSize = 1024;
            var receivedDataBuffer = new ArraySegment<Byte>(new Byte[maxMessageSize]);
            var cancellationToken = new CancellationToken();

            while (_webSocket.State == WebSocketState.Open)
            {
                WebSocketReceiveResult webSocketReceiveResult = await _webSocket.ReceiveAsync(receivedDataBuffer, cancellationToken);

                if (webSocketReceiveResult.MessageType == WebSocketMessageType.Close)
                {
                    await _webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, String.Empty, cancellationToken);
                }
                else
                {
                    byte[] payloadData = receivedDataBuffer.Array.Where(b => b != 0).ToArray();
                    string receiveString = Encoding.UTF8.GetString(payloadData, 0, payloadData.Length);
                    var newString = String.Format("Hello, " + receiveString + " ! Time {0}", DateTime.Now);
                    Byte[] bytes = Encoding.UTF8.GetBytes(newString);
                    await _webSocket.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true, cancellationToken);
                }
            }
        }

        public static async Task Send(string message)
        {
            if (_webSocket?.State == WebSocketState.Open)
            {
                var cancellationToken = new CancellationToken();
                byte[] buffer = Encoder.GetBytes(message);

                await _webSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, cancellationToken);
            }
            else
            {
                Debug.WriteLine("Web socket failed to send message");
            }
        }
    }
}
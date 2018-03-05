using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.WebSockets;
using MankalaHTML.Models;
using MankalaLib;
using Newtonsoft.Json;
using BoxName = MankalaLib.BoxName;
using Game = MankalaHTML.Models.Game;

namespace MankalaHTML
{
    public class WebSocketHandlerA : IHttpHandler
    {
        private static readonly UTF8Encoding Encoder = new UTF8Encoding();
        private readonly MankalaDatabase _db = MankalaDatabase.Instance;
        private static readonly IDictionary<string, WebSocket> WebSockets = new Dictionary<string, WebSocket>();

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
            var webSocket = webSocketContext.WebSocket;
            const int maxMessageSize = 1024;
            var receivedDataBuffer = new ArraySegment<Byte>(new Byte[maxMessageSize]);
            var cancellationToken = new CancellationToken();

            while (webSocket.State == WebSocketState.Open)
            {
                WebSocketReceiveResult webSocketReceiveResult = await webSocket.ReceiveAsync(receivedDataBuffer, cancellationToken);

                if (webSocketReceiveResult.MessageType == WebSocketMessageType.Close)
                {
                    await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, String.Empty, cancellationToken);
                }
                else
                {
                    byte[] payloadData = receivedDataBuffer.Array.Where(b => b != 0).ToArray();
                    string receiveString = Encoding.UTF8.GetString(payloadData, 0, payloadData.Length);
                    var gameIdentifier = JsonConvert.DeserializeObject<GameIdentifier>(receiveString);
                    WebSockets[$"{gameIdentifier.BoxName}--{gameIdentifier.ID}"] = webSocket;
                    var game = _db.GetGame(gameIdentifier.ID);
                    if (WebSockets.ContainsKey($"{BoxName.AG}--{gameIdentifier.ID}") && WebSockets.ContainsKey($"{BoxName.BG}--{gameIdentifier.ID}"))
                    {
                        game.Message = "AG's turn!";
                        var mgame = GetModelsGame(game);
                        var mgameJson = JsonConvert.SerializeObject(mgame);
                        await Send(WebSockets[$"{BoxName.AG}--{gameIdentifier.ID}"], mgameJson);
                        await Send(WebSockets[$"{BoxName.BG}--{gameIdentifier.ID}"], mgameJson);
                    }
                }
            }
        }

        private static async Task Send(WebSocket webSocket, string message)
        {
            if (webSocket?.State == WebSocketState.Open)
            {
                var cancellationToken = new CancellationToken();
                byte[] buffer = Encoder.GetBytes(message);
                await webSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, cancellationToken);
            }
            else
            {
                Debug.WriteLine("Web socket failed to send message");
            }
        }

        public static async Task Send(GameIdentifier gameIdentifier, string message)
        {
            var webSocket = WebSockets[$"{gameIdentifier.BoxName}--{gameIdentifier.ID}"];
            if (webSocket?.State == WebSocketState.Open)
            {
                var cancellationToken = new CancellationToken();
                byte[] buffer = Encoder.GetBytes(message);
                await webSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, cancellationToken);
            }
            else
            {
                Debug.WriteLine("Web socket failed to send message");
            }
        }

        private Game GetModelsGame(MankalaLib.Game game)
        {
            return new Game
            {
                ID = game.ID,
                A1 = game.A1.Value,
                A2 = game.A2.Value,
                A3 = game.A3.Value,
                A4 = game.A4.Value,
                A5 = game.A5.Value,
                A6 = game.A6.Value,
                AG = game.AG.Value,
                B1 = game.B1.Value,
                B2 = game.B2.Value,
                B3 = game.B3.Value,
                B4 = game.B4.Value,
                B5 = game.B5.Value,
                B6 = game.B6.Value,
                BG = game.BG.Value,
                Message = game.Message
            };
        }
    }
}
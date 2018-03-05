using System;
using System.Diagnostics;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MankalaHTML.Models;
using MankalaLib;
using Newtonsoft.Json;
using BoxName = MankalaHTML.Models.BoxName;
using Game = MankalaLib.Game;

namespace MankalaHTML
{
    public class MankalaMachine
    {
        private ClientWebSocket _socket;
        private CancellationTokenSource cts = new CancellationTokenSource();

        public async Task StartAsync(Uri requestUri, int machineId)
        {

            // receive loop
            await Task.Factory.StartNew(
                async () =>
                {
                    var rcvBytes = new byte[500];
                    var rcvBuffer = new ArraySegment<byte>(rcvBytes);

                    Machine machine = new Machine();
                    var wsUri = new Uri($"ws://{requestUri.Host}:{requestUri.Port}/WebSocketHandlerA.ashx");
                    _socket = new ClientWebSocket();
                    _socket.ConnectAsync(wsUri, cts.Token).Wait();
                    var player = new Player { BoxName = BoxName.BG, GameId = machineId };
                    var jsonId = JsonConvert.SerializeObject(player);
                    await SendAsync(jsonId);
                    while (true)
                    {
                        var rcvResult = await _socket.ReceiveAsync(rcvBuffer, cts.Token);
                        var msgBytes = rcvBuffer.Skip(rcvBuffer.Offset).Take(rcvResult.Count).ToArray();
                        var rcvMsg = Encoding.UTF8.GetString(msgBytes);
                        var mgame = JsonConvert.DeserializeObject<Models.Game>(rcvMsg);
                        var game = GetGame(mgame);
                        if (game.Message != "BG's turn!") continue;
                        var mbox = await machine.Play(game);
                        var endpoint = $"{requestUri.Scheme}://{requestUri.Host}:{requestUri.Port}/api/games/{game.ID}";
                        var parameters = "";
                        var contentType = "application/json";
                        //body = $"{{ \"ID\": {game.ID}, \"BoxName\": {mbox.Name}, \"Reset\": false}}";
                        var click = new Click { ID = game.ID, BoxName = (BoxName)mbox, Reset = false };
                        var body = JsonConvert.SerializeObject(click);
                        var client = new RestClient();
                        var json = client.MakeRequest(endpoint, parameters, HttpVerb.PUT, body, contentType);
                        Debug.WriteLine("Received: {0}", json);
                    }
                }, cts.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default);
        }

        public async Task SendAsync(string message)
        {
            byte[] sendBytes = Encoding.UTF8.GetBytes(message);
            var sendBuffer = new ArraySegment<byte>(sendBytes);
            await _socket.SendAsync(sendBuffer, WebSocketMessageType.Text, endOfMessage: true, cancellationToken: cts.Token);
        }

        public void Stop()
        {
            cts.Cancel();
        }

        private Game GetGame(Models.Game mgame)
        {
            var game = new Game
            {
                ID = mgame.ID
            };
            game.A1.Value = mgame.A1;
            game.A2.Value = mgame.A2;
            game.A3.Value = mgame.A3;
            game.A4.Value = mgame.A4;
            game.A5.Value = mgame.A5;
            game.A6.Value = mgame.A6;
            game.AG.Value = mgame.AG;
            game.B1.Value = mgame.B1;
            game.B2.Value = mgame.B2;
            game.B3.Value = mgame.B3;
            game.B4.Value = mgame.B4;
            game.B5.Value = mgame.B5;
            game.B6.Value = mgame.B6;
            game.BG.Value = mgame.BG;
            game.Message = mgame.Message;
            return game;
        }
    }
}
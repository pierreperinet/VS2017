using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using MankalaHTML.Models;
using MankalaLib;
using Newtonsoft.Json;
using BoxName = MankalaLib.BoxName;
using Game = MankalaHTML.Models.Game;

namespace MankalaHTML.Controllers
{
    public class GamesController : ApiController
    {
        private MankalaDatabase db = MankalaDatabase.Instance;

        // GET: api/Games
        public IList<Game> Get()
        {
            return db.GetGames().Select(GetModelsGame).ToList();
        }

        // GET: api/Games/5
        public Game Get(int id)
        {
            var game = db.GetGame(id);
            return GetModelsGame(game);
        }

        // POST: api/Games
        public HttpResponseMessage Post()
        {
            var game = db.AddGame();
            if (game == null)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
            var response = Request.CreateResponse(HttpStatusCode.Created, GetModelsGame(game), "application/json");
            response.Headers.Location = new Uri(Request.RequestUri, $"Games/{game.ID}");
            return response;
        }

        // PUT: api/Games/5
        public async Task<HttpResponseMessage> Put(int id, [FromBody] Click mclick)
        {
            var game = db.GetGame(id);
            if (game == null)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
            if (mclick.Reset)
            {
                game.Reset();
                game.Message = "AG's turn!";
                await SendGame(Models.BoxName.AG, game);
                await SendGame(Models.BoxName.BG, game);
                var response1 = Request.CreateResponse(HttpStatusCode.OK, GetModelsGame(game), "application/json");
                response1.Headers.Location = new Uri(Request.RequestUri, $"Games/{game.ID}");
                return response1;
            }
            var box = game.GetBox((BoxName)mclick.BoxName);
            var next = box.Click(box.Name);
            var winner = game.GetWinner(next);
            if (winner != null)
            {
                game.Message = $"{winner} wins!";
            }
            else
            {
                game.Message = $"{next}'s turn!";
            }
            await SendGame(Models.BoxName.AG, game);
            await SendGame(Models.BoxName.BG, game);
            var response = Request.CreateResponse(HttpStatusCode.Accepted, GetModelsGame(game), "application/json");
            response.Headers.Location = new Uri(Request.RequestUri, $"Games/{game.ID}");
            return response;
        }

        // DELETE: api/Games/5
        public void Delete(int id)
        {
            db.DeleteGame(id);
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

        private async Task SendGame(Models.BoxName boxName, MankalaLib.Game game)
        {
            var mgame = GetModelsGame(game);
            var requestJson = JsonConvert.SerializeObject(mgame);
            await WebSocketHandlerA.Send(new Player { GameId = game.ID, BoxName = boxName }, requestJson);
        }
    }
}
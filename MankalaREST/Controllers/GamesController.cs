using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MankalaLib;
using MankalaREST.Models;
using BoxName = MankalaLib.BoxName;
using Game = MankalaREST.Models.Game;

namespace MankalaREST.Controllers
{
    public class GamesController : ApiController
    {
        private MankalaDatabase db = MankalaDatabase.Instance;

        // GET: api/Games
        public IList<Game> Get()
        {
            return db.GetGames().Select(game => GetModelsGame(game)).ToList();
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
        public void Put(int id, [FromBody] Click mclick)
        {
            var game = db.GetGame(id);
            if (game == null)
            {
                return;
            }
            var box = game.GetBox((BoxName)mclick.BoxName);
            box.Click(box.Name);
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
                BG = game.BG.Value
            };
        }
    }
}
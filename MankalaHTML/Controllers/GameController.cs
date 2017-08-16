using System.Web.Mvc;
using MankalaLib;
using BoxName = MankalaHTML.Models.BoxName;
using Game = MankalaHTML.Models.Game;

namespace MankalaHTML.Controllers
{
    [Authorize]
    public class GameController : Controller
    {
        private readonly MankalaDatabase _db = MankalaDatabase.Instance;

        // GET: Game
        public ActionResult Index()
        {
            return base.View();
        }

        // GET: Game/0
        public ActionResult Index(int id)
        {
            return base.View(id);
        }

        //
        // GET: /Game/View
        [AllowAnonymous]
        public new ActionResult View()
        {
            return base.View();
        }

        //
        // GET: /Game/New
        [HttpGet]
        [AllowAnonymous]
        public ActionResult New()
        {
            ViewBag.StatusMessage = "Game new";
            var game = _db.AddGame();
            game.Message = "AG's turn!";
            return RedirectToAction("Play", new {game.ID });
        }

        //
        // GET: /Game/Rules
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Rules()
        {
            ViewBag.StatusMessage = "Mankala Rules";
            return View();
        }


        //
        // GET: /Game/Play/0
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Play(int id)
        {
            var game = _db.GetGame(id);
            var mgame = GetModelsGame(game);
            return View("Play", mgame);
        }

        //
        // GET: /Game/PlayB/0
        [HttpGet]
        [AllowAnonymous]
        public ActionResult PlayB(int id)
        {
            var game = _db.GetGame(id);
            var mgame = GetModelsGame(game);
            return View("PlayB", mgame);
        }

        //
        // GET: /Game/Reset/0
        [AllowAnonymous]
        public ActionResult Reset(int id)
        {
            var game = _db.GetGame(id);
            game.Reset();
            game.Message = "AG's turn!";
            return RedirectToAction("Play", new {game.ID });
        }

        //
        // POST: /Game/Play/0?boxName=A1
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Play(int id, BoxName boxName)
        {
            var game = _db.GetGame(id);
            var mboxName = (MankalaLib.BoxName)boxName;
            var box = game.GetBox(mboxName);
            box.Click(mboxName);
            return RedirectToAction("Play", new {game.ID });
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
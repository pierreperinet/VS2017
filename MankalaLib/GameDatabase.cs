using System.Collections.Generic;
using System.Linq;

namespace MankalaLib
{
    public sealed class MankalaDatabase
    {
        public IList<Game> Games { get; set; }

        private MankalaDatabase()
        {
            Games = new List<Game>();
        }

        public static MankalaDatabase Instance { get; } = new MankalaDatabase();

        public Game AddGame()
        {
            var game = new Game();
            Games.Add(game);
            game.ID = Games.IndexOf(game);
            return game;
        }

        public Game GetGame(int id)
        {
            return Games.FirstOrDefault(g => g.ID == id);
        }

        public IList<Game> GetGames()
        {
            return Games;
        }

        public void DeleteGame(int id)
        {
            Games.RemoveAt(id);
        }
    }
}
using System;
using System.Linq;
using System.Threading.Tasks;
using MankalaLib;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var Machine = new Machine();
            var Game = new Game();

            var mbox = Machine.Play(Game).Result;
            Console.WriteLine($"Box to click : {mbox.Name}");
            Console.Read();
        }
    }

    public class Machine
    {
        public async Task<Box> Play(Game game)
        {
            await Task.Delay(1);

            var mbox = PlayA(game);
            return game.GetBox(mbox);
        }

        private const int level = 5;

        private BoxName PlayA(Game xgame)
        {
            BoxName click = BoxName.A1;
            var gain = 0;

            if (xgame.A1.Value != 0)
            {
                var game = GetGame(xgame);
                var next = game.A1.Click(BoxName.AG);
                game.Steps.Add(BoxName.A1);
                PlayBoxA(ref click, ref gain, game, next);
            }

            if (xgame.A2.Value != 0)
            {
                var game = GetGame(xgame);
                var next = game.A2.Click(BoxName.AG);
                game.Steps.Add(BoxName.A2);
                PlayBoxA(ref click, ref gain, game, next);
            }

            if (xgame.A3.Value != 0)
            {
                var game = GetGame(xgame);
                var next = game.A3.Click(BoxName.AG);
                game.Steps.Add(BoxName.A3);
                PlayBoxA(ref click, ref gain, game, next);
            }

            if (xgame.A4.Value != 0)
            {
                var game = GetGame(xgame);
                var next = game.A4.Click(BoxName.AG);
                game.Steps.Add(BoxName.A4);
                PlayBoxA(ref click, ref gain, game, next);
            }

            if (xgame.A5.Value != 0)
            {
                var game = GetGame(xgame);
                var next = game.A5.Click(BoxName.AG);
                game.Steps.Add(BoxName.A5);
                PlayBoxA(ref click, ref gain, game, next);
            }

            if (xgame.A6.Value != 0)
            {
                var game = GetGame(xgame);
                var next = game.A6.Click(BoxName.AG);
                game.Steps.Add(BoxName.A6);
                PlayBoxA(ref click, ref gain, game, next);
            }

            return click;
        }

        private BoxName PlayB(Game xgame)
        {
            BoxName click = BoxName.B1;
            var gain = 0;

            if (xgame.B1.Value != 0)
            {
                var game = GetGame(xgame);
                var next = game.B1.Click(BoxName.BG);
                game.Steps.Add(BoxName.B1);
                PlayBoxB(ref click, ref gain, game, next);
            }

            if (xgame.B2.Value != 0)
            {
                var game = GetGame(xgame);
                var next = game.B2.Click(BoxName.BG);
                game.Steps.Add(BoxName.B2);
                PlayBoxB(ref click, ref gain, game, next);
            }

            if (xgame.B3.Value != 0)
            {
                var game = GetGame(xgame);
                var next = game.B3.Click(BoxName.BG);
                game.Steps.Add(BoxName.B3);
                PlayBoxB(ref click, ref gain, game, next);
            }

            if (xgame.B4.Value != 0)
            {
                var game = GetGame(xgame);
                var next = game.B4.Click(BoxName.BG);
                game.Steps.Add(BoxName.B4);
                PlayBoxB(ref click, ref gain, game, next);
            }

            if (xgame.B5.Value != 0)
            {
                var game = GetGame(xgame);
                var next = game.B5.Click(BoxName.BG);
                game.Steps.Add(BoxName.B5);
                PlayBoxB(ref click, ref gain, game, next);
            }

            if (xgame.B6.Value != 0)
            {
                var game = GetGame(xgame);
                var next = game.B6.Click(BoxName.BG);
                game.Steps.Add(BoxName.B6);
                PlayBoxB(ref click, ref gain, game, next);
            }

            return click;
        }

        private void PlayBoxA(ref BoxName click, ref int gain, Game game, BoxName next)
        {
            var winner = game.GetWinner(next);
            if (winner == null)
            {
                if (game.Steps.Count >= level)
                {
                    var g = game.GetAGains();
                    if (g > game.GetBGains())
                    {
                        if (g > gain)
                        {
                            gain = g;
                            click = game.Steps.First();
                            ConsoleWriteLine(game);
                        }
                    }
                }
                else
                {
                    if (next == BoxName.BG)
                    {
                        PlayB(game);
                    }
                    else
                    {
                        PlayA(game);
                    }
                }
            }
            else if (winner == BoxName.AG)
            {
                var g = game.GetAGains();
                if (g > game.GetBGains())
                {
                    if (g > gain)
                    {
                        gain = g;
                        click = game.Steps.First();
                        ConsoleWriteLine(game);
                    }
                }
            }
        }

        private void PlayBoxB(ref BoxName click, ref int gain, Game game, BoxName next)
        {
            var winner = game.GetWinner(next);
            if (winner == null)
            {
                if (game.Steps.Count >= level)
                {
                    var g = game.GetBGains();
                    if (g > game.GetAGains())
                    {
                        if (g > gain)
                        {
                            gain = g;
                            click = game.Steps.First();
                            ConsoleWriteLine(game);
                        }
                    }
                }
                else
                {
                    if (next == BoxName.BG)
                    {
                        PlayB(game);
                    }
                    else
                    {
                        PlayA(game);
                    }
                }
            }
            else if (winner == BoxName.BG)
            {
                var g = game.GetBGains();
                if (g > game.GetAGains())
                {
                    if (g > gain)
                    {
                        gain = g;
                        click = game.Steps.First();
                        ConsoleWriteLine(game);
                    }
                }
            }
        }

        private static void ConsoleWriteLine(Game game)
        {
            Console.WriteLine($"{game.GetAGains()} + {game.GetBGains()} = {game.GetAGains() + game.GetBGains()} - {string.Join(",", game.Steps)}");
        }

        private Game GetGame(Game mgame)
        {
            var game = new Game
            {
                ID = mgame.ID
            };
            game.A1.Value = mgame.A1.Value;
            game.A2.Value = mgame.A2.Value;
            game.A3.Value = mgame.A3.Value;
            game.A4.Value = mgame.A4.Value;
            game.A5.Value = mgame.A5.Value;
            game.A6.Value = mgame.A6.Value;
            game.AG.Value = mgame.AG.Value;
            game.B1.Value = mgame.B1.Value;
            game.B2.Value = mgame.B2.Value;
            game.B3.Value = mgame.B3.Value;
            game.B4.Value = mgame.B4.Value;
            game.B5.Value = mgame.B5.Value;
            game.B6.Value = mgame.B6.Value;
            game.BG.Value = mgame.BG.Value;
            game.Message = mgame.Message;
            game.Steps = mgame.Steps.ToList();
            return game;
        }

        static int Max(params int[] numbers)
        {
            return numbers.Max();
        }
    }

}

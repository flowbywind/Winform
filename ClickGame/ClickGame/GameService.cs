using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClickGame
{
   public   class GameService
    {
        public static void RecordGame(int duration, int click)
        {
            var Game = new Game()
            {
                 Id = Guid.NewGuid().ToString(),
                 Clicks=click,
                 Duration=duration,
                 ClickPerSecond=click/duration,
                 Played=DateTime.Now
            };
            using (var db = new GameContext())
            {
                db.Games.Add(Game);
                int nums = db.SaveChanges();
                Console.WriteLine(nums);
            }
        }
        public static IEnumerable<Game> GetTopGames()
        {
            using (var db = new GameContext())
            {
                var topGames = db.Games
                    .OrderByDescending(g => g.ClickPerSecond)
                    .Take(3)
                    .ToList();
                return topGames;
            }
        }
    }
}

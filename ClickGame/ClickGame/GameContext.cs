using Microsoft.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace ClickGame
{
    public class Game
    {
        [System.ComponentModel.DataAnnotations.Key]
        public string Id { get; set; }
        public int Duration { get; set; }
        public int Clicks { get; set; }
        public double ClickPerSecond { get; set; }
        public DateTime Played { get; set; }
    }
    public class GameContext :DbContext
    {
        public DbSet<Game> Games { get; set; }
        /// <summary>
        /// 配置创建数据库
        /// </summary>
        /// <param name="options"></param>
        protected override void OnConfiguring(DbContextOptions options)
        {
            ///只需使用sqllite即可
            options.UseSQLite("Filename=game3.db");
        }
    }
}

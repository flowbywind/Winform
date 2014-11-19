using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace ClickGame
{
   
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            //创建数据库
            using (var db = new GameContext())
            {
                //如果创建了，则不会重新创建
                bool flag = db.Database.EnsureCreated();
                Console.WriteLine(flag);
            }
            ReadData();
        }
        void ReadData()
        {
            lblInfo.Text = "前三名成绩";
            foreach (var game in GameService.GetTopGames())
            {
                lblInfo.Text += game.ClickPerSecond + "  ";
            }
        }
        private static List<Game> _Games = new List<Game>();
        private static int nums=0;
        private void BtnOk_Click(object sender, EventArgs e)
        {
            nums = nums + 1;
            while (nums > new Random().Next(3, 7))
            {
                GameService.RecordGame(3, nums);
                nums = 0;
                lblInfo.Text = "前三名成绩";
                ReadData();
            }
        }

    }
}

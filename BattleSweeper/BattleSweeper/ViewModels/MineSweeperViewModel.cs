using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Games.MineSweeper;

namespace BattleSweeper.ViewModels
{
    public class MineSweeperViewModel : ViewModelBase
    {
        public string Greeting => "Welcome to Avalonia!";

        public MineSweeperViewModel() 
        {
        }
        public void constructGame(IMineSweeper mineGame)
        {
            //mineGame.Tiles

        }

    }
}

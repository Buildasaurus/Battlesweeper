using BattleSweeper.Models;
using Games.MineSweeper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Drawing;
using System.Diagnostics;

namespace BattleSweeper.ViewModels
{
    public class GameViewModel : ViewModelBase
    {
        

        public GameViewModel()
        {
            GameModel _game = new GameModel();
            IMineSweeper mine_sweeper_model = MineSweeperFactory.construct<MineSweeper>(new(10, 10), 10);

        }

        
    }   
}

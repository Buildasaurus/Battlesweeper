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
using ReactiveUI;
using System.Diagnostics;

namespace BattleSweeper.ViewModels
{
    public class GameViewModel : ViewModelBase
    {
        public ViewModelBase GameView { get => m_game_view; set => this.RaiseAndSetIfChanged(ref m_game_view, value); }


        public GameViewModel()
        {
            GameModel _game = new GameModel();
            IMineSweeper mine_sweeper_model = MineSweeperFactory.construct<MineSweeper>(new(10, 10), 10);

        }
        
        protected ViewModelBase m_game_view;
    }   
}

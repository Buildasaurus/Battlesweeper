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
            IMineSweeper mine_sweeper_model = MineSweeperFactory.construct<MineSweeper>(new(10, 10), 10, 13);

            MineSweeperViewModel mine_sweeper_vm = new(mine_sweeper_model, 60);
            mine_sweeper_vm.start();

            GameView = mine_sweeper_vm;

            mine_sweeper_vm.leftClickTile(new Point(3, 3));
            mine_sweeper_vm.leftClickTile(new Point(6, 6));
            mine_sweeper_vm.leftClickTile(new Point(0, 2));
            mine_sweeper_vm.leftClickTile(new Point(0, 0));
            mine_sweeper_vm.leftClickTile(new Point(1, 0));
            mine_sweeper_vm.leftClickTile(new Point(2, 0));
            mine_sweeper_vm.leftClickTile(new Point(4, 0));
            mine_sweeper_vm.leftClickTile(new Point(5, 1));
            mine_sweeper_vm.leftClickTile(new Point(6, 0));
            mine_sweeper_vm.leftClickTile(new Point(6, 1));
            mine_sweeper_vm.leftClickTile(new Point(6, 2));
            mine_sweeper_vm.leftClickTile(new Point(7, 1));
            mine_sweeper_vm.leftClickTile(new Point(7, 2));
            mine_sweeper_vm.leftClickTile(new Point(9, 3));
            mine_sweeper_vm.leftClickTile(new Point(9, 4));
            mine_sweeper_vm.leftClickTile(new Point(8, 4));
            mine_sweeper_vm.leftClickTile(new Point(9, 5));
            mine_sweeper_vm.leftClickTile(new Point(8, 5));
            mine_sweeper_vm.leftClickTile(new Point(7, 6));
            mine_sweeper_vm.leftClickTile(new Point(9, 6));
            mine_sweeper_vm.leftClickTile(new Point(7, 7));
            mine_sweeper_vm.leftClickTile(new Point(8, 7));
            mine_sweeper_vm.leftClickTile(new Point(9, 7));
            mine_sweeper_vm.leftClickTile(new Point(7, 8));
            mine_sweeper_vm.leftClickTile(new Point(9, 8));
            mine_sweeper_vm.rightClickTile(new Point(0, 9));
            mine_sweeper_vm.rightClickTile(new Point(0, 1));
            mine_sweeper_vm.rightClickTile(new Point(0, 3));
            mine_sweeper_vm.rightClickTile(new Point(3, 0));
            mine_sweeper_vm.rightClickTile(new Point(5, 0));
            mine_sweeper_vm.rightClickTile(new Point(5, 2));
            mine_sweeper_vm.rightClickTile(new Point(5, 6));
            mine_sweeper_vm.rightClickTile(new Point(7, 9));
            mine_sweeper_vm.rightClickTile(new Point(8, 3));
            mine_sweeper_vm.rightClickTile(new Point(8, 6));
        }
        
        protected ViewModelBase m_game_view;
    }   
}

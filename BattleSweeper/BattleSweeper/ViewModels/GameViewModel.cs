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
using EventHandler = BattleSweeper.Models.EventHandler;
using Avalonia.Input;
using Avalonia.Input.Raw;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia;
using Size = System.Drawing.Size;

namespace BattleSweeper.ViewModels
{
    public class GameViewModel : ViewModelBase
    {
        public ViewModelBase GameView { get => m_game_view; set => this.RaiseAndSetIfChanged(ref m_game_view, value); }

        static Window window = (App.Current.ApplicationLifetime as ClassicDesktopStyleApplicationLifetime).MainWindow;

        public GameViewModel()
        {
            EventHandler.start();
            Size gridSize = new Size(10, 10);
            //create 10x10 grid, with 10 bombs.
            IMineSweeper mine_sweeper_model = MineSweeperFactory.construct<MineSweeper>(gridSize, 10);
            //create the view model, with a 60 second timer.
            MineSweeperViewModel mine_sweeper_vm = new(mine_sweeper_model, 60);
            EventHandler.KeyChanged += (x =>
            {
            });
            EventHandler.MouseChanged += (x =>
            {
                System.Drawing.Point field = coordToField(mine_sweeper_vm.Position, x.MousePosition);
                if (x.button == MouseButton.Right)
                {
                    mine_sweeper_vm.rightClickTile(field);
                }
                if (x.button == MouseButton.Left)
                {
                    if (mine_sweeper_vm.grid.inBounds(field))
                    {
                        mine_sweeper_vm.leftClickTile(field);
                    }
                }

                /*Get the current mouse position.
                Point cursorPos = new Point();
                GetCursorPos(ref cursorPos);
                mousePosition = new Point(cursorPos.X, cursorPos.Y);
                MouseChanged?.Invoke(new MouseArgs((MouseButton)c.Type, mousePosition));*/
            });
            


            mine_sweeper_vm.start();

            GameView = mine_sweeper_vm;
        }

        public System.Drawing.Point coordToField(Rect gridCoord, System.Drawing.Point mousePos)
        {
            double windowX = window.Position.X;
            double windowY = window.Position.Y + 30;
            double x;
            double y;

            //calc x and y coordinat relative to topleft
            x = mousePos.X - gridCoord.TopLeft.X - windowX;
			y = mousePos.Y - gridCoord.TopLeft.Y - windowY;


            //calculate corresponding tile
            double tilewidth = gridCoord.Width/10;
            double tileHeight = gridCoord.Height / 10;

            int xtile = (int)Math.Floor(x / tilewidth);
            int ytile = (int)Math.Floor(y / tileHeight);
            return new System.Drawing.Point(xtile, ytile);
        }

        protected ViewModelBase m_game_view;
    }   
}

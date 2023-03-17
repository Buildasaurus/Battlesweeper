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
                Trace.WriteLine(x.key);                
            });
            EventHandler.MouseChanged += (x =>
            {
                if (x.button == MouseButton.Right)
                {
                    mine_sweeper_vm.rightClickTile(coordToField(gridSize, x.MousePosition));
                }
                if (x.button == MouseButton.Left)
                {
                    Point point = coordToField(gridSize, x.MousePosition);
                    if (mine_sweeper_vm.grid.inBounds(point))
                    {
                        mine_sweeper_vm.leftClickTile(point);
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

        public Point coordToField(Size Size, Point mousePos)
        {
            double width = window.Width;
            double height = window.Height;
            double windowX = window.Position.X;
            double windowY = window.Position.Y + 30;
            double gridSize = Math.Min(width, height);
            double x;
            double y;
            if (gridSize == width) //if grid touches left wall
            {
                x = mousePos.X - windowX;
                y = mousePos.Y - windowY;
            }
            else
            {
                x = mousePos.X-windowX - (width - gridSize)/2;
                y = mousePos.Y - windowY - (height - gridSize) / 2;
            }

            if (x < 0 || y < 0)
            {
                return new Point(-1, -1);
            }
            double tilewidth = 60;
            double tileHeight = 60;
            int xtile = (int)Math.Floor((double)mousePos.X / tilewidth);
            int ytile = (int)Math.Floor((double)mousePos.Y / tileHeight);
            return new Point(xtile, ytile);
        }

        protected ViewModelBase m_game_view;
    }   
}

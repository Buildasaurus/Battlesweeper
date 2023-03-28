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
using Avalonia.Platform;
using System.Windows.Forms;
using System.Windows;
using Screen = System.Windows.Forms.Screen;
using Rect = Avalonia.Rect;
using Window = Avalonia.Controls.Window;
using Games.Battleships;

namespace BattleSweeper.ViewModels
{
    public class GameViewModel : ViewModelBase
    {
        public ViewModelBase GameView { get => m_game_view; set => this.RaiseAndSetIfChanged(ref m_game_view, value); }
        static Window window = (App.Current.ApplicationLifetime as ClassicDesktopStyleApplicationLifetime).MainWindow;
        public bool shiftPressed = false;
        int minesweepergame = 1;
        MineSweeperViewModel mine_sweeper_vm;
        public GameViewModel()
        {
            EventHandler.start();
            mine_sweeper_vm = constructMineField();
            EventHandler.KeyChanged += (x =>
            {
                if(x.key == Key.LeftShift)
                {
                    shiftPressed = !shiftPressed;
                    Trace.WriteLine(shiftPressed);
                }
            });
            EventHandler.MouseChanged += (x =>
            {
                System.Drawing.Point field = coordToField(mine_sweeper_vm.Position, x.MousePosition);

                if (mine_sweeper_vm.grid.inBounds(field))
                {
                    if (x.button == MouseButton.Middle)
                    {
                        mine_sweeper_vm.rightClickTile(field);
                    }
                    if (x.button == MouseButton.Left)
                    {

                        if ((x.modifier & RawInputModifiers.Shift) == RawInputModifiers.None)
                        {
                            mine_sweeper_vm.leftClickTile(field);
                        }
                        else
                        {
                            mine_sweeper_vm.leftShiftClickTile(field);
                        }
                    }
                }
            });
            


            mine_sweeper_vm.start();

            GameView = mine_sweeper_vm;
        }

        public void gameover (object? s, bool foundAllBombs)
        {
            if (minesweepergame == 1) //if first game, start the next one
            {
                MineSweeperTransitionViewModel transition = new MineSweeperTransitionViewModel();
                GameView = transition;

                transition.TransitionFinished.Subscribe(x =>
                {
                    mine_sweeper_vm.GameOver -= gameover;
                    mine_sweeper_vm = constructMineField();
                    mine_sweeper_vm.start();
                    GameView = mine_sweeper_vm;

                    minesweepergame++;
                });      
                
            }
            else
            {
                IBattleships battle = new BattleshipsTemplate();
                BattleShipsViewModel battleshipgame = new BattleShipsViewModel();
                Trace.WriteLine("play battleships");
            }
        }

        private MineSweeperViewModel constructMineField()
        {
            Size gridSize = new Size(10, 10);
            //create 10x10 grid, with 10 bombs.
            IMineSweeper mine_sweeper_model = MineSweeperFactory.construct<MineSweeper>(gridSize, 10);
            //create the view model, with a 60 second timer.
            MineSweeperViewModel mineSweeperVM = new(mine_sweeper_model, 60);
            mineSweeperVM.GameOver += gameover;
            return mineSweeperVM;
        }

        public System.Drawing.Point coordToField(Rect gridCoord, System.Drawing.Point mousePos)
        {
            double scale = (Screen.PrimaryScreen.Bounds.Width / SystemParameters.PrimaryScreenWidth); 
            double windowX = window.Position.X;
            double windowY = window.Position.Y + 30;
            double x;
            double y;

            //calc x and y coordinat relative to topleft
            x = mousePos.X / scale - gridCoord.TopLeft.X - windowX / scale;
			y = mousePos.Y / scale - gridCoord.TopLeft.Y - windowY / scale;
            /*
            Trace.WriteLine(gridCoord.TopLeft.X);
            */

            //calculate corresponding tile
            double tilewidth = gridCoord.Width/10;
            double tileHeight = gridCoord.Height / 10;

            int xtile = (int)Math.Floor(x / tilewidth);
            int ytile = (int)Math.Floor(y / tileHeight);
            /*
            Trace.WriteLine(gridCoord.X + " " + gridCoord.Y);
            Trace.WriteLine(mousePos.X + " " + mousePos.Y);
            Trace.WriteLine(windowX + " " + windowY);
            Trace.WriteLine("x and y: " + x + " " + y + " tilewidth and height: " + tilewidth + " " + tileHeight + " x and y tile: " + xtile + " " + ytile);
            */
            return new System.Drawing.Point(xtile, ytile);
        }

        protected ViewModelBase m_game_view;
    }   
}

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
using Windows.UI.WebUI;
using Point = System.Drawing.Point;

namespace BattleSweeper.ViewModels
{
    public class GameViewModel : ViewModelBase
    {
        public ViewModelBase GameView { get => m_game_view; set => this.RaiseAndSetIfChanged(ref m_game_view, value); }
        static Window window = (App.Current.ApplicationLifetime as ClassicDesktopStyleApplicationLifetime).MainWindow;
        public bool shiftPressed = false;
        public bool RPressed = false;
        int minesweepergame = 1;
        MineSweeperViewModel mine_sweeper_vm;
        HashSet<Point> pl1Bombs;
        HashSet<Point> pl2Bombs;

        IBattleships pl1;
        IBattleships pl2;
        BattleShipsViewModel battleshipgame;

        public GameViewModel()
        {
            EventHandler.start();
            mine_sweeper_vm = constructMineField();

            EventHandler.KeyChanged += minesweeperKeyChanged;
            EventHandler.MouseChanged += minesweeperMouseEvent;

            mine_sweeper_vm.start();

            GameView = mine_sweeper_vm;
        }

        void minesweeperKeyChanged(KeyArgs x)
        {
            if (x.key == Key.LeftShift)
            {
                shiftPressed = !shiftPressed;
                Trace.WriteLine(shiftPressed);
            }
        }

        void minesweeperMouseEvent(MouseArgs x)
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
        }

        void battleshipsKeyEvent(KeyArgs x) //for rotation
        {
            if (x.key == Key.R)
            {
                if (!RPressed)
                {
                    Trace.WriteLine("changing direction");
                    battleshipgame.changeDirection();
                }
                RPressed = !RPressed;
            }
        }


        void battleshipsMouseEvent(MouseArgs x)
        {
            Rect? rect = battleshipgame.ActiveGridBounds;
            if (rect != null)
            {
                System.Drawing.Point field = coordToField((Rect)rect, x.MousePosition);

                if (battleshipgame.bs1_tile_vm.inBounds(field))
                {
                    if (x.button == MouseButton.Left)
                    {
                        battleshipgame.leftClick(field);
                    }
                }
            }
        }

        public void gameover (object? s, bool foundAllBombs)
        {
            Trace.WriteLine("game over - found all bombs: " + foundAllBombs);
            if (minesweepergame == 1) //if first game, start the next one
            {
                MineSweeperTransitionViewModel transition = new MineSweeperTransitionViewModel();
                GameView = transition;
                pl1Bombs = mine_sweeper_vm.mine_sweeper.CurrentBombs;

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
                pl2Bombs = mine_sweeper_vm.mine_sweeper.CurrentBombs;

                pl1 = new Battleships();
                pl2 = new Battleships();

                pl1.constructBoard(pl1Bombs.ToList());
                pl2.constructBoard(pl2Bombs.ToList());
                battleshipgame = new BattleShipsViewModel(pl1, pl2);

                mine_sweeper_vm.GameOver -= gameover;
                EventHandler.KeyChanged -= minesweeperKeyChanged;
                EventHandler.MouseChanged -= minesweeperMouseEvent;
                EventHandler.MouseChanged += battleshipsMouseEvent;
                EventHandler.KeyChanged += battleshipsKeyEvent;

                




                //BattleshipsTemplate pl1_t = new BattleshipsTemplate();
                //BattleshipsTemplate pl2_t = new BattleshipsTemplate();

                //pl1 = pl1_t;
                //pl2 = pl2_t;

                //pl1_t.constructBoard(new());

                //pl1_t.setTile(new(3, 3), new(0, false, false, false, true, false));
                //pl1_t.setTile(new(3, 4), new(0, false, false, false, false, false));
                //pl1_t.setTile(new(3, 5), new(0, true, true, false, false, false));
                //pl1_t.setTile(new(3, 6), new(0, false, false, false, false, true));

                //pl1_t.setTile(new(4, 8), new(0, false, false, true, true, false));
                //pl1_t.setTile(new(5, 8), new(0, true, false, true, false, false));
                //pl1_t.setTile(new(6, 8), new(0, false, false, true, false, false));
                //pl1_t.setTile(new(7, 8), new(0, false, false, true, false, true));

                //pl1_t.setTile(new(1, 1), new(-1, true, false, false, false, false));

                //pl2.constructBoard(new());
                GameView = battleshipgame;

            }
        }

        private MineSweeperViewModel constructMineField()
        {
            Size gridSize = new Size(10, 10);
            //create 10x10 grid, with 10 bombs.
            IMineSweeper mine_sweeper_model = MineSweeperFactory.construct<MineSweeper>(gridSize, 10);
            //create the view model, with a 60 second timer.
            MineSweeperViewModel mineSweeperVM = new(mine_sweeper_model, 30);
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

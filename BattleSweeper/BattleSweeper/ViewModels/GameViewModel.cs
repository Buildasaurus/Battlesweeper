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

namespace BattleSweeper.ViewModels
{
    public class GameViewModel : ViewModelBase
    {
        [DllImport("user32.dll")]
        static extern bool GetCursorPos(ref Point lpPoint);

        public ViewModelBase GameView { get => m_game_view; set => this.RaiseAndSetIfChanged(ref m_game_view, value); }


        public GameViewModel()
        {
            GameModel _game = new GameModel();
            IMineSweeper mine_sweeper_model = MineSweeperFactory.construct<MineSweeper>(new(10, 10), 10);


            // Create a new thread to continuously monitor the mouse for input.
            mouseThread = new Thread(new ThreadStart(MouseThread));
            mouseThread.Start();
            MouseThread();
        }

        private void MouseThread()
        {
            while (isRunning)
            {
                // Get the current mouse position.
                Point cursorPos = new Point();
                GetCursorPos(ref cursorPos);

                // Do something with the mouse position.
                Console.WriteLine($"Mouse position: {cursorPos.X}, {cursorPos.Y}");

                // Sleep for a short amount of time to prevent the loop from running too frequently.
                Thread.Sleep(10);
            }
        }
        // Call this method to stop the thread and terminate the loop.
        public void Stop()
        {
            isRunning = false;
            mouseThread.Join();
        }
        
        protected ViewModelBase m_game_view;
        private Thread mouseThread;
        private bool isRunning = true;
    }   
}

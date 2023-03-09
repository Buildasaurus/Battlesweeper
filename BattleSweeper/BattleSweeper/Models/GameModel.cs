using Avalonia.Input;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Threading;

namespace BattleSweeper.Models
{
    public class GameModel
    {

        [DllImport("user32.dll")]
        static extern bool GetCursorPos(ref Point lpPoint);

        private Thread mouseThread;
        private bool isRunning = true;
        public GameModel()
        {
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
    }
}

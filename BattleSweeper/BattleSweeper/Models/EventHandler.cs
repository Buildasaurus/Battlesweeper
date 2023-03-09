using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Drawing;
using Avalonia.Input;

namespace BattleSweeper.Models
{
    static public class EventHandler
    {
        [DllImport("user32.dll")]
        static extern bool GetCursorPos(ref Point lpPoint);

        private static Thread mouseThread;
        private static bool isRunning = true;

        public static void start()
        {
            // Create a new thread to continuously monitor the mouse for input.
            mouseThread = new Thread(MouseThread);
            mouseThread.Start();


        }
        public static Action<EventArgs>? MouseChanged;

        private static void MouseThread()
        {
            while (isRunning)
            {
                // Get the current mouse position.
                Point cursorPos = new Point();
                GetCursorPos(ref cursorPos);

                // Do something with the mouse position.
                Trace.WriteLine($"Mouse position: {cursorPos.X}, {cursorPos.Y}");
                MouseChanged?.Invoke(new EventArgs(Key.A, MouseButton.Left, new Point(cursorPos.X, cursorPos.Y)));
                // Sleep for a short amount of time to prevent the loop from running too frequently.
                Thread.Sleep(10);
            }
        }
        // Call this method to stop the thread and terminate the loop.
        public static void Stop()
        {
            isRunning = false;
            mouseThread.Join();
        }
    }
    public class EventArgs
    {
        Key key;
        MouseButton button;
        Point MousePosition;
        public EventArgs(Key key, MouseButton button, Point MousePosition)
        {
            this.key = key;
            this.button = button;
            this.MousePosition = MousePosition;
        }

    }
}

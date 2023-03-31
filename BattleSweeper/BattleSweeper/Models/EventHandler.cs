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
using System.Windows.Input;
using System.Reactive.Disposables;
using Avalonia.Input.Raw;
using System.Security.Cryptography;
using MouseButton = Avalonia.Input.MouseButton;
using InputManager = Avalonia.Input.InputManager;
using Key = Avalonia.Input.Key;

namespace BattleSweeper.Models
{
    static public class EventHandler
    {
        [DllImport("user32.dll")]
        static extern bool GetCursorPos(ref Point lpPoint);

        private static Thread mouseThread;
        private static bool isRunning = true;
        private static bool shiftPressed = false;
        public static Point mousePosition = new Point();
        public static Action<MouseArgs>? MouseChanged;
        public static Action<KeyArgs>? KeyChanged;
        public static Action<MousePosition>? MouseMoved;



        public static void start()
        {
            // Create a new thread to continuously monitor the mouse for input.
            InputManager.Instance.PostProcess.Subscribe(x => {

                if (x is RawKeyEventArgs b)
                {
                    KeyChanged?.Invoke(new KeyArgs(b.Key));
                }
                if (x is RawPointerEventArgs c)
                {
                    MouseMoved?.Invoke(new MousePosition(new Point((int)c.Position.X, (int)c.Position.Y)));
                    if (c.InputModifiers != RawInputModifiers.None)
                    {
                        // Get the current mouse position
                        Point cursorPos = new Point();
                        GetCursorPos(ref cursorPos);
                        mousePosition = new Point(cursorPos.X, cursorPos.Y);
                        MouseChanged?.Invoke(new MouseArgs((MouseButton)c.Type, mousePosition, c.InputModifiers));
                    }
                }
            });
        }
    }

    public class MousePosition
    {
        public Point Position;
        public MousePosition(Point Position)
        {
            this.Position = Position;
        }
    }

    public class MouseArgs
    {
        public RawInputModifiers modifier;
        public MouseButton button;
        public Point MousePosition;
        public MouseArgs(MouseButton button, Point MousePosition, RawInputModifiers modifier)
        {
            this.modifier = modifier;
            this.button = button;
            this.MousePosition = MousePosition;
        }

    }
    public class KeyArgs
    {
        public Key key;
        bool isDown;
        public KeyArgs(Key key)
        {
            this.key = key;
        }
    }
}

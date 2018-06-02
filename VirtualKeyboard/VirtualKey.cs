using BaseX;
using FrooxEngine;
using FrooxEngine.UI;
using System;
using System.Runtime.InteropServices;

namespace VirtualKeyboard
{
    public class VirtualKey : Component, IButtonReceiver
    {
        // TODO!!! Simulate key
        public readonly Sync<Key> TargetKey;
        public readonly Sync<string> AppendString;

        public readonly Sync<Key> ShiftTargetKey;
        public readonly Sync<string> ShiftAppendString;

        public readonly RelayRef<VirtualShift> ShiftKey;
        public readonly RelayRef<VirtualDesktop> DesktopKey;

        Text text;
        private static int WM_CHAR = 0x102;
        private static int WM_SETTEXT = 0xC;
        private static int WM_KEYDOWN = 0x100;
        private static int WM_KEYUP = 0x101;
        private static int WM_LBUTTONDOWN = 0x201;
        private static int WM_LBUTTONUP = 0x202;
        private static int WM_CLOSE = 0x10;
        private static int WM_COMMAND = 0x111;
        private static int WM_CLEAR = 0x303;
        private static int WM_DESTROY = 0x2;
        private static int WM_GETTEXT = 0xD;
        private static int WM_GETTEXTLENGTH = 0xE;
        private static int WM_LBUTTONDBLCLK = 0x203;

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool PostMessage(IntPtr hWnd, int Msg, int wParam, IntPtr lParam);

//        [DllImport("User32.dll")]
//        public static extern int SendMessage(IntPtr hWnd, int uMsg, int wParam, string lParam);

        public void Pressed(Button button)
        {
            IntPtr hWndCurrent = GetForegroundWindow();
            if (button.PressingUser.IsLocal)
            {
                string append;
                Key key;
                if (ShiftKey.Target == null || !ShiftKey.Target.Shift)
                {
                    append = AppendString.Value;
                    key = TargetKey.Value;
                }
                else
                {
                    append = ShiftAppendString.Value;
                    key = ShiftTargetKey.Value;
                }

                if (key != Key.None)
                {
                    if (DesktopKey.Target == null || !DesktopKey.Target.Shift)
                    {
                        Input.SimulatePress(key);
                    }
                    else
                    {
                        if (key == Key.Return)
                        {
                            PostMessage(hWndCurrent, WM_KEYDOWN, 0xD, IntPtr.Zero);
                        }
                        else if (key == Key.Backspace)
                        {
                            PostMessage(hWndCurrent, WM_KEYDOWN, 0x8, IntPtr.Zero);
                        }
                        else if (key == Key.LeftArrow)
                        {
                            PostMessage(hWndCurrent, WM_KEYDOWN, 0x25, IntPtr.Zero);
                        }
                        else if (key == Key.UpArrow)
                        {
                            PostMessage(hWndCurrent, WM_KEYDOWN, 0x26, IntPtr.Zero);
                        }
                        else if (key == Key.RightArrow)
                        {
                            PostMessage(hWndCurrent, WM_KEYDOWN, 0x27, IntPtr.Zero);
                        }
                        else if (key == Key.DownArrow)
                        {
                            PostMessage(hWndCurrent, WM_KEYDOWN, 0x28, IntPtr.Zero);
                        }
                        else if (key == Key.Tab)
                        {
                            PostMessage(hWndCurrent, WM_KEYDOWN, 0x9, IntPtr.Zero);
                        }
                        else if (key == Key.Escape)
                        {
                            PostMessage(hWndCurrent, WM_KEYDOWN, 0x1B, IntPtr.Zero);
                        }
                    }
                }

                if (append != null && append.Length > 0)
                {
                    if (DesktopKey.Target == null || !DesktopKey.Target.Shift)
                    {
                        Input.TypeAppend(append);
                    }
                    else
                    {
                        if (append.Length == 1)
                        {
                            int chr = (int)append[0];
                            if (chr >= 0x20 && chr <= 0x7e)
                            {
                                PostMessage(hWndCurrent, WM_CHAR, chr, IntPtr.Zero);
                            }
                        }
                    }
                }
                ShiftKey.Target?.KeyPressed();
            }
        }

        public void Released(Button button) { }

        protected override void OnChanges()
        {
            if (ShiftKey.Target != null)
            {
                if (text == null)
                    text = Slot.GetComponentInChildren<Text>();

                // update shift status
                text.Content_Field.Value = ShiftKey.Target.Shift ? ShiftAppendString.Value : AppendString.Value;
            }
        }
    }
}

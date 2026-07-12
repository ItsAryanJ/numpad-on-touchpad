using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace TouchpadNumpad.Core
{
    public static class ClickBlocker
    {
        private const int WH_MOUSE_LL = 14;
        private const int WM_LBUTTONDOWN = 0x0201;
        private const int WM_LBUTTONUP = 0x0202;
        private const int WM_RBUTTONDOWN = 0x0204;
        private const int WM_RBUTTONUP = 0x0205;
        private const int WM_MOUSEMOVE = 0x0200;
        public static DateTime LastMouseMoveTime { get; private set; } = DateTime.MinValue;
        public static DateTime LastTouchpadContactTime { get; set; } = DateTime.MinValue;
        private static Point _lastCursorPos;
        private static LowLevelMouseProc _proc = HookCallback;
        private static IntPtr _hookID = IntPtr.Zero;
        public static bool IsBlocking { get; set; } = false;

        public static void Start()
        {
            if (_hookID == IntPtr.Zero)
            {
                _lastCursorPos = Cursor.Position;
                _hookID = SetHook(_proc);
            }
        }

        public static void Stop()
        {
            if (_hookID != IntPtr.Zero)
            {
                UnhookWindowsHookEx(_hookID);
                _hookID = IntPtr.Zero;
            }
        }

        private static IntPtr SetHook(LowLevelMouseProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule!)
            {
                return SetWindowsHookEx(WH_MOUSE_LL, proc, GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        private delegate IntPtr LowLevelMouseProc(int nCode, IntPtr wParam, IntPtr lParam);

        private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            // If blocking is active, and the action is a Left or Right click, drop it.
            if (nCode >= 0)
            {
                int msg = (int)wParam;
                if (msg == WM_MOUSEMOVE)
                {
                    Point currentPos = Cursor.Position;
                    int dx = Math.Abs(currentPos.X - _lastCursorPos.X);
                    int dy = Math.Abs(currentPos.Y - _lastCursorPos.Y);

                    // Ignore < 5 pixel movements that happen naturally when tapping
                    if (dx > 5 || dy > 5)
                    {
                        LastMouseMoveTime = DateTime.Now;
                        _lastCursorPos = currentPos;
                    }
                }
                if (IsBlocking)
                {
                    if (msg == WM_LBUTTONDOWN || msg == WM_LBUTTONUP ||
                        msg == WM_RBUTTONDOWN || msg == WM_RBUTTONUP)
                    {
                        if ((DateTime.Now - LastTouchpadContactTime).TotalMilliseconds < 300)
                        {
                            return (IntPtr)1;
                        }
                    }
                }
            }

            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelMouseProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);
    }
}

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace TouchpadNumpad.Services
{
    public static class KeyboardService
    {
        [DllImport("user32.dll", SetLastError = true)]
        static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, int dwExtraInfo);
        public static void PressKey(byte keyCode)
        {
            KeyDown(keyCode);
            KeyUp(keyCode);
        }

        public static void KeyDown(byte keyCode)
        {
            keybd_event(keyCode, 0, 0, 0);
        }

        public static void KeyUp(byte keyCode)
        {
            keybd_event(keyCode, 0, 2, 0);
        }
    }


}

using System;
using System.Runtime.InteropServices;

namespace TouchpadNumpad.Services
{
    public static class KeyboardService
    {
        public static void PressKey(byte keyCode)
            => InputService.PressKey(keyCode);

        public static void KeyDown(byte keyCode)
            => InputService.KeyDown(keyCode);

        public static void KeyUp(byte keyCode)
            => InputService.KeyUp(keyCode);
    }

}

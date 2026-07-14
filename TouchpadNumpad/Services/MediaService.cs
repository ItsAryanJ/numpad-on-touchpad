using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace TouchpadNumpad.Services
{
    public static class MediaService
    {
        [DllImport("user32.dll", SetLastError = true)]
        private static extern void keybd_event(
        byte bVk,
        byte bScan,
        uint dwFlags,
        int dwExtraInfo);

        private static void PressMediaKey(byte key)
        {
            keybd_event(key, 0, 0, 0);
            keybd_event(key, 0, 2, 0);
        }

        private const byte VK_VOLUME_MUTE = 0xAD;
        private const byte VK_VOLUME_DOWN = 0xAE;
        private const byte VK_VOLUME_UP = 0xAF;

        private const byte VK_MEDIA_NEXT_TRACK = 0xB0;
        private const byte VK_MEDIA_PREV_TRACK = 0xB1;
        private const byte VK_MEDIA_PLAY_PAUSE = 0xB3;

        public static void VolumeUp()
        {
            PressMediaKey(VK_VOLUME_UP);
        }

        public static void VolumeDown()
        {
            PressMediaKey(VK_VOLUME_DOWN);
        }

        public static void Mute()
        {
            PressMediaKey(VK_VOLUME_MUTE);
        }

        public static void PlayPause()
        {
            PressMediaKey(VK_MEDIA_PLAY_PAUSE);
        }

        public static void NextTrack()
        {
            PressMediaKey(VK_MEDIA_NEXT_TRACK);
        }

        public static void PreviousTrack()
        {
            PressMediaKey(VK_MEDIA_PREV_TRACK);
        }
    }
}

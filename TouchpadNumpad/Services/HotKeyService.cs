using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace TouchpadNumpad.Services;

public static class HotkeyService
{
    [DllImport("user32.dll")]
    private static extern short GetAsyncKeyState(int vKey);
    private static bool IsPressed(byte key)
    {
        return (GetAsyncKeyState(key) & 0x8000) != 0;
    }
    private static byte? GetVirtualKey(string key)
    {
        return key switch
        {
            "Ctrl" => 0x11,
            "Shift" => 0x10,
            "Alt" => 0x12,
            "Win" => 0x5B,

            _ => KeyDefinitions.KeyboardKeys.TryGetValue(
                    key,
                    out byte vk)
                ? vk
                : null
        };
    }
    public static bool IsShortcutPressed(IEnumerable<string> shortcut)
    {
        foreach (string key in shortcut)
        {
            byte? vk = GetVirtualKey(key);

            if (vk == null)
                return false;

            if (!IsPressed(vk.Value))
                return false;
        }

        return true;
    }
    public static void ReleaseShortcut(IEnumerable<string> shortcut)
    {
        foreach (string key in shortcut)
        {
            byte? vk = GetVirtualKey(key);

            if (vk != null)
            {
                KeyboardService.KeyUp(vk.Value);
            }
        }
    }
}
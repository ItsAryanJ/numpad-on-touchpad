using Microsoft.Win32;
using System.Reflection;

namespace TouchpadNumpad.Services;

public static class StartupService
{
    private const string AppName = "Touchpad Numpad";

    public static void SetStartup(bool enabled)
    {
        using RegistryKey? key =
            Registry.CurrentUser.OpenSubKey(
                @"Software\Microsoft\Windows\CurrentVersion\Run",
                true);

        if (key == null)
            return;

        if (enabled)
        {
            key.SetValue(
                AppName,
                $"\"{Assembly.GetEntryAssembly()!.Location}\"");
        }
        else
        {
            key.DeleteValue(AppName, false);
        }
    }

    public static bool IsStartupEnabled()
    {
        using RegistryKey? key =
            Registry.CurrentUser.OpenSubKey(
                @"Software\Microsoft\Windows\CurrentVersion\Run");

        return key?.GetValue(AppName) != null;
    }
}
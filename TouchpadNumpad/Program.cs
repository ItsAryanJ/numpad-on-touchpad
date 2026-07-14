using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Windows.Forms;
using TouchpadNumpad;
using TouchpadNumpad.Core;
using TouchpadNumpad.Models;
using TouchpadNumpad.Services;

[SupportedOSPlatform("windows")]

class Program
{
    static int lastRow = -1;
    static int lastCol = -1;
    static System.Threading.Timer? resetTimer;
    static bool numpadEnabled = false;
    static DateTime lastToggleTime = DateTime.MinValue;

    [DllImport("user32.dll")]
    static extern short GetAsyncKeyState(int vKey);

    public static void ToggleNumpad()
    {
        numpadEnabled = !numpadEnabled;

        ClickBlocker.IsBlocking = numpadEnabled;
        TrayManager.UpdateToggleText(numpadEnabled);

        lastToggleTime = DateTime.Now;

        KeyboardService.KeyUp(0x11);
        KeyboardService.KeyUp(0x10);
        KeyboardService.KeyUp(0x7B);

        string flyoutPath = FlyoutManager.GetFlyoutPath();

        if (File.Exists(flyoutPath))
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = flyoutPath,
                Arguments = numpadEnabled ? "enabled" : "disabled",
                UseShellExecute = true
            });
        }

    }

    [STAThread]
    static void Main()
    {
        bool createdNew;
        Mutex mutex = new Mutex(
            true,
            "TouchpadNumpad_SingleInstance",
            out createdNew);

        if (!createdNew)
        {
            return;
        }

        SettingsManager settingsManager = new();
        AppState.Settings = settingsManager.Load();

        ProfileManager profileManager = new();
        AppState.CurrentProfile = profileManager.LoadProfile(AppState.Settings.ActiveProfile);

        ClickBlocker.Start();
        TouchpadHandler handler = new(0);

        if (!handler.IsTouchpadExists())
        {
            Console.WriteLine("Touchpad not found");
            return;
        }

        handler.Touched += (_, contacts) =>
        {
            if (contacts.Length > 0)
            {
                ClickBlocker.LastTouchpadContactTime = DateTime.Now;
            }

            if ((DateTime.Now - lastToggleTime).TotalMilliseconds > 500)
            {
                bool ctrl = (GetAsyncKeyState(0x11) & 0x8000) != 0;
                bool shift = (GetAsyncKeyState(0x10) & 0x8000) != 0;
                bool f12 = (GetAsyncKeyState(0x7B) & 0x8000) != 0;

                if (ctrl && shift && f12)
                {
                    ToggleNumpad();
                }
            }

            if (!numpadEnabled)
                return;

            if (contacts.Length == 0)
                return;

            if ((DateTime.Now - ClickBlocker.LastMouseMoveTime).TotalMilliseconds < 200)
            {
                lastRow = -1;
                lastCol = -1;
                return;
            }

            var c = contacts[0];

            int col = Math.Min(c.X * AppState.CurrentProfile.Columns / c.MaxX, AppState.CurrentProfile.Columns - 1);
            int row = Math.Min(c.Y * AppState.CurrentProfile.Rows / c.MaxY, AppState.CurrentProfile.Rows - 1);

            GridCell? cell = AppState.CurrentProfile.Cells.FirstOrDefault(
                c => c.Row == row &&
                     c.Column == col);

            if (cell == null)
                return;

            if (row == lastRow && col == lastCol)
                return;

            lastRow = row;
            lastCol = col;

            resetTimer?.Dispose();
            resetTimer = new System.Threading.Timer(_ =>
            {
                lastRow = -1;
                lastCol = -1;
            }, null, 200, System.Threading.Timeout.Infinite);

            ActionExecutor.Execute(cell.Action);

            Task.Run(async () =>
            {
                await Task.Delay(50);

            });
        };

        handler.StartCapture();

        TrayManager.CreateTrayIcon(ToggleNumpad);

        Application.Run();

        ClickBlocker.Stop();
    }
}
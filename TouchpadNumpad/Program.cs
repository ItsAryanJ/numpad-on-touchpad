using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.IO;
using System.Linq;
using TouchpadNumpad.Core;
using TouchpadNumpad.Services;

class Program
{
    static int lastRow = -1;
    static int lastCol = -1;
    static System.Threading.Timer? resetTimer;
    static bool numpadEnabled = false;
    static DateTime lastToggleTime = DateTime.MinValue;
    static NotifyIcon? trayIcon;
    static ToolStripMenuItem? toggleMenuItem;

    [DllImport("user32.dll")]
    static extern short GetAsyncKeyState(int vKey);
    
    [DllImport("user32.dll", SetLastError = true)]
    static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, int dwExtraInfo);

    static void SimulateNumpadKey(byte keyCode)
    {
        keybd_event(keyCode, 0, 0, 0);
        keybd_event(keyCode, 0, 2, 0);
    }
    
    static void CreateTrayIcon()
    {
        toggleMenuItem = new ToolStripMenuItem("Enable Numpad");

        toggleMenuItem.Click += (_, __) =>
        {
            numpadEnabled = !numpadEnabled;
            ClickBlocker.IsBlocking = numpadEnabled;

            toggleMenuItem.Text =
                numpadEnabled ? "Disable Numpad" : "Enable Numpad";

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
        };

        var exitItem = new ToolStripMenuItem("Exit");

        exitItem.Click += (_, __) =>
        {
            trayIcon?.Dispose();
            Application.Exit();
        };

        var menu = new ContextMenuStrip();
        menu.Items.Add(toggleMenuItem);
        menu.Items.Add(new ToolStripSeparator());
        menu.Items.Add(exitItem);

        string iconPath = Path.Combine(
        AppDomain.CurrentDomain.BaseDirectory,
        "assets",
        "numpad.ico");

        trayIcon = new NotifyIcon
        {
            Icon = File.Exists(iconPath)
                ? new Icon(iconPath)
                : SystemIcons.Application,
            Visible = true,
            Text = "Touchpad Numpad",
            ContextMenuStrip = menu
        };
    }
    static void ShowPopup(string message)
    {
        Form popup = new Form();

        popup.FormBorderStyle = FormBorderStyle.None;
        popup.StartPosition = FormStartPosition.Manual;
        popup.Size = new Size(300, 80);
        popup.TopMost = true;
        popup.ShowInTaskbar = false;
        popup.BackColor = Color.FromArgb(30, 30, 30);
        popup.Opacity = 0.9;

        var screen = Screen.PrimaryScreen!.WorkingArea;

        popup.Location = new Point(
            (screen.Width - popup.Width) / 2,
            screen.Height - popup.Height - 80
        );

        Label label = new Label();
        label.Text = message;
        label.Dock = DockStyle.Fill;
        label.ForeColor = Color.White;
        label.TextAlign = ContentAlignment.MiddleCenter;
        label.Font = new Font("Segoe UI", 16, FontStyle.Bold);

        popup.Controls.Add(label);

        var timer = new System.Windows.Forms.Timer();
        timer.Interval = 1000;

        timer.Tick += (_, __) =>
        {
            timer.Stop();
            popup.Close();
            popup.Dispose();
        };

        popup.Shown += (_, __) => timer.Start();

        popup.Show();
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

        ClickBlocker.Start();
        var handler = new TouchpadHandler(0);

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
                    numpadEnabled = !numpadEnabled;
                    ClickBlocker.IsBlocking = numpadEnabled;

                    lastToggleTime = DateTime.Now;

                    keybd_event(0x11, 0, 2, 0); 
                    keybd_event(0x10, 0, 2, 0); 
                    keybd_event(0x7B, 0, 2, 0); 

                    if (toggleMenuItem != null)
                    {
                        toggleMenuItem.Text = numpadEnabled ? "Disable Numpad" : "Enable Numpad";
                    }

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

            int col = Math.Min(c.X * 3 / c.MaxX, 2);
            int row = Math.Min(c.Y * 4 / c.MaxY, 3);

            string[,] layout =
            {
            { "7", "8", "9" },
            { "4", "5", "6" },
            { "1", "2", "3" },
            { "0", "0", "." }
        };

            string key = layout[row, col];

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

            switch (key)
            {
                case "0": SimulateNumpadKey(0x60); break;
                case "1": SimulateNumpadKey(0x61); break;
                case "2": SimulateNumpadKey(0x62); break;
                case "3": SimulateNumpadKey(0x63); break;
                case "4": SimulateNumpadKey(0x64); break;
                case "5": SimulateNumpadKey(0x65); break;
                case "6": SimulateNumpadKey(0x66); break;
                case "7": SimulateNumpadKey(0x67); break;
                case "8": SimulateNumpadKey(0x68); break;
                case "9": SimulateNumpadKey(0x69); break;
                case ".": SimulateNumpadKey(0x6E); break;
            }

            Task.Run(async () =>
            {
                await Task.Delay(50);

            });
        };

        handler.StartCapture();

        CreateTrayIcon();

        Application.Run();
        ClickBlocker.Stop();
    }
}
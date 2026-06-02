using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.IO;
using System.Linq;

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
    public static class ClickBlocker
    {
        private const int WH_MOUSE_LL = 14;
        private const int WM_LBUTTONDOWN = 0x0201;
        private const int WM_LBUTTONUP = 0x0202;
        private const int WM_RBUTTONDOWN = 0x0204;
        private const int WM_RBUTTONUP = 0x0205;
        private const int WM_MOUSEMOVE = 0x0200;
        public static DateTime LastMouseMoveTime { get; private set; } = DateTime.MinValue;
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
                        return (IntPtr)1;
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
    static void CreateTrayIcon()
    {
        toggleMenuItem = new ToolStripMenuItem("Enable Numpad");

        toggleMenuItem.Click += (_, __) =>
        {
            numpadEnabled = !numpadEnabled;

            toggleMenuItem.Text =
                numpadEnabled ? "Disable Numpad" : "Enable Numpad";

            string flyoutPath = Path.GetFullPath(
                Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory,
                    Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory,
                    "NumpadFlyout.exe")));

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
        ClickBlocker.Start();
        var handler = new TouchpadHandler(0);

        if (!handler.IsTouchpadExists())
        {
            Console.WriteLine("Touchpad not found");
            return;
        }

        handler.Touched += (_, contacts) =>
        {
            if ((DateTime.Now - lastToggleTime).TotalMilliseconds > 500)
            {
                bool ctrl = (GetAsyncKeyState(0x11) & 0x8000) != 0;
                bool shift = (GetAsyncKeyState(0x10) & 0x8000) != 0;
                bool alt = (GetAsyncKeyState(0x12) & 0x8000) != 0;
                bool f12 = (GetAsyncKeyState(0x7B) & 0x8000) != 0;

                if (ctrl && shift && alt && f12)
                {
                    numpadEnabled = !numpadEnabled;
                    ClickBlocker.IsBlocking = numpadEnabled;

                    lastToggleTime = DateTime.Now;
                    string flyoutPath = Path.GetFullPath(
                    Path.Combine(
                        AppDomain.CurrentDomain.BaseDirectory,
                        @"..\..\..\..\NumpadFlyout\bin\Debug\net10.0-windows\NumpadFlyout.exe"));

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
                case "0": SendKeys.SendWait("0"); break;
                case "1": SendKeys.SendWait("1"); break;
                case "2": SendKeys.SendWait("2"); break;
                case "3": SendKeys.SendWait("3"); break;
                case "4": SendKeys.SendWait("4"); break;
                case "5": SendKeys.SendWait("5"); break;
                case "6": SendKeys.SendWait("6"); break;
                case "7": SendKeys.SendWait("7"); break;
                case "8": SendKeys.SendWait("8"); break;
                case "9": SendKeys.SendWait("9"); break;
                case ".": SendKeys.SendWait("."); break;
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

using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using TouchpadNumpad.Core;

namespace TouchpadNumpad.Services
{
    public static class TrayManager
    {
        private static NotifyIcon? trayIcon;
        private static ToolStripMenuItem? toggleMenuItem;

        public static void UpdateToggleText(bool enabled)
        {
            if (toggleMenuItem != null)
                toggleMenuItem.Text = enabled
                    ? "Disable Numpad"
                    : "Enable Numpad";
        }

        public static void Dispose()
        {
            trayIcon?.Dispose();
        }
        public static void CreateTrayIcon(Action toggleAction)
        {
            toggleMenuItem = new ToolStripMenuItem("Enable Numpad");


            toggleMenuItem.Click += (_, __) => toggleAction();

            var exitItem = new ToolStripMenuItem("Exit");

            exitItem.Click += (_, __) =>
            {
                Dispose();
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
    }
}

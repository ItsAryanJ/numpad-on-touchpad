using System;
using System.Windows;

namespace NumpadFlyout
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            bool enabled =
                e.Args.Length > 0 &&
                e.Args[0].Equals(
                    "enabled",
                    StringComparison.OrdinalIgnoreCase);

            MainWindow window = new MainWindow(enabled);
            window.Show();
        }
    }
}
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace NumpadFlyout
{
    public partial class MainWindow : Window
    {
        public MainWindow() : this(true)
        {
        }

        public MainWindow(bool enabled)
        {
            InitializeComponent();

            if (enabled)
            {
                StatusText.Text = "Numpad Enabled";
                StatusBar.Fill = new SolidColorBrush(
                    (Color)ColorConverter.ConvertFromString("#FFB900"));
            }
            else
            {
                StatusText.Text = "Numpad Disabled";
                StatusBar.Fill = new SolidColorBrush(
                    (Color)ColorConverter.ConvertFromString("#666666"));
            }

            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var workArea = SystemParameters.WorkArea;

            Left = workArea.Left + (workArea.Width - Width) / 2;

            double finalTop = workArea.Top + workArea.Height - Height - 16;

            Top = finalTop + 20;
            Opacity = 0;

            var slideIn = new DoubleAnimation
            {
                From = finalTop + 20,
                To = finalTop,
                Duration = TimeSpan.FromMilliseconds(300),
                EasingFunction = new CubicEase
                {
                    EasingMode = EasingMode.EaseOut
                }
            };

            var fadeIn = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromMilliseconds(300)
            };

            BeginAnimation(Window.TopProperty, slideIn);
            BeginAnimation(Window.OpacityProperty, fadeIn);

            var timer = new System.Windows.Threading.DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(2000)
            };

            timer.Tick += (_, __) =>
            {
                timer.Stop();

                var slideOut = new DoubleAnimation
                {
                    From = finalTop,
                    To = finalTop + 20,
                    Duration = TimeSpan.FromMilliseconds(300),
                    EasingFunction = new CubicEase
                    {
                        EasingMode = EasingMode.EaseIn
                    }
                };

                var fadeOut = new DoubleAnimation
                {
                    From = 1,
                    To = 0,
                    Duration = TimeSpan.FromMilliseconds(300)
                };

                fadeOut.Completed += (_, __) => Close();

                BeginAnimation(Window.TopProperty, slideOut);
                BeginAnimation(Window.OpacityProperty, fadeOut);
            };

            timer.Start();
        }
    }
}
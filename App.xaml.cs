using System;
using System.Windows;

namespace NekoBeats
{
    public partial class App : Application
    {
        public static VisualizerWindow Visualizer { get; private set; }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            // Ensure application lifetime is tied to the main control panel
            ShutdownMode = ShutdownMode.OnMainWindowClose;

            try
            {
                // Open Control Panel first and set it as the application's MainWindow
                var mainWindow = new MainWindow();
                MainWindow = mainWindow;
                mainWindow.Show();

                // Create Visualizer and make it owned by the control panel so it won't become the app's main window
                Visualizer = new VisualizerWindow
                {
                    Owner = mainWindow
                };

                // Preserve requested window state, but avoid showing while maximized with ShowActivated=false
                var desiredState = Visualizer.WindowState;

                // We want the visualizer not to steal activation if possible
                // But WPF forbids ShowActivated = false when WindowState == Maximized.
                // Workaround: if it's maximized, temporarily set to Normal before Show() then restore.
                if (desiredState == WindowState.Maximized)
                {
                    // temporarily normalize, prevent stealing activation, show, then maximize
                    Visualizer.WindowState = WindowState.Normal;
                    Visualizer.ShowActivated = false;
                    Visualizer.Show();

                    // restore maximized after showing (may cause a visual resize flash but avoids exception)
                    Visualizer.WindowState = WindowState.Maximized;
                }
                else
                {
                    // safe to avoid activation steal
                    Visualizer.ShowActivated = false;
                    Visualizer.Show();

                    // restore requested state if not Normal
                    if (desiredState != WindowState.Normal)
                    {
                        Visualizer.WindowState = desiredState;
                    }
                }
            }
            catch (Exception ex)
            {
                // Surface startup exception so you can see the real problem immediately
                MessageBox.Show($"Startup error: {ex.Message}\n\n{ex.StackTrace}", "NekoBeats - Startup Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Shutdown();
            }
        }
    }
}

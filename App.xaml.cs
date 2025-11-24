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

                // Create Visualizer after main window shown, make it owned so it won't become the app's main window
                Visualizer = new VisualizerWindow
                {
                    Owner = mainWindow,
                    ShowActivated = false // avoid stealing focus
                };
                Visualizer.Show();
            }
            catch (Exception ex)
            {
                // Surface startup exception so you can see the real problem immediately
                MessageBox.Show($"Startup error: {ex.Message}\n\n{ex.StackTrace}", "NekoBeats - Startup Error", MessageBoxButton.OK, MessageBoxImage.Error);
                // If startup fails, shut down to avoid partial state
                Shutdown();
            }
        }
    }
}

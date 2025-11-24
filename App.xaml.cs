using System.Windows;

namespace NekoBeats
{
    public partial class App : Application
    {
        public static VisualizerWindow Visualizer { get; private set; }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            // Make the control panel the main window so app lifetime is tied to it
            ShutdownMode = ShutdownMode.OnMainWindowClose;

            // Open Control Panel first and set it as the application's MainWindow
            var mainWindow = new MainWindow();
            MainWindow = mainWindow;
            mainWindow.Show();

            // Create Visualizer and make it owned by the control panel so it won't become the app's main window
            Visualizer = new VisualizerWindow();
            Visualizer.Owner = mainWindow;
            // Don't let the visualizer steal activation when it is shown
            Visualizer.ShowActivated = false;
            Visualizer.Show();
        }
    }
}

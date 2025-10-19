using System.Windows;

namespace NekoBeats
{
    public partial class App : Application
    {
        public static VisualizerWindow Visualizer { get; private set; }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            // Open Visualizer Overlay first
            Visualizer = new VisualizerWindow();
            Visualizer.Show();
            
            // Open Control Panel
            var mainWindow = new MainWindow();
            mainWindow.Show();
        }
    }
}

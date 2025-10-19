using System.Windows;
using System.Windows.Input;

namespace NekoBeats
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void GradientBars_Click(object sender, RoutedEventArgs e)
        {
            ThemeManager.CurrentTheme = new GradientBarsTheme();
            StatusText.Text = "🎵 Theme: Gradient Bars";
        }

        private void Purrticles_Click(object sender, RoutedEventArgs e)
        {
            // Create placeholder theme for now
            ThemeManager.CurrentTheme = new GradientBarsTheme(); // Temporary
            StatusText.Text = "🎵 Theme: Purr-ticles (Coming Soon)";
        }

        private void RetroArcade_Click(object sender, RoutedEventArgs e)
        {
            // Create placeholder theme for now
            ThemeManager.CurrentTheme = new GradientBarsTheme(); // Temporary
            StatusText.Text = "🎵 Theme: Retro Arcade (Coming Soon)";
        }

        private void SpaceNebula_Click(object sender, RoutedEventArgs e)
        {
            // Create placeholder theme for now
            ThemeManager.CurrentTheme = new GradientBarsTheme(); // Temporary
            StatusText.Text = "🎵 Theme: Space Nebula (Coming Soon)";
        }

        private void DragVisualizer_Click(object sender, RoutedEventArgs e)
        {
            if (App.Visualizer != null)
            {
                App.Visualizer.DragMove();
            }
        }

        private void ToggleVisibility_Click(object sender, RoutedEventArgs e)
        {
            if (App.Visualizer != null)
            {
                App.Visualizer.Visibility = App.Visualizer.Visibility == Visibility.Visible 
                    ? Visibility.Hidden 
                    : Visibility.Visible;
            }
        }

        private void ResetPosition_Click(object sender, RoutedEventArgs e)
        {
            if (App.Visualizer != null)
            {
                App.Visualizer.Left = 100;
                App.Visualizer.Top = 100;
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            // Clean up when control panel closes
            App.Visualizer?.Close();
            base.OnClosed(e);
        }
    }
}

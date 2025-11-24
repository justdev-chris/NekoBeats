using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace NekoBeats
{
    public partial class VisualizerWindow : Window
    {
        private DispatcherTimer renderTimer;
        private AudioProcessor audioProcessor;
        private float[] currentFrequencies = new float[32];
        private double[] currentFFT = new double[0];

        public VisualizerWindow()
        {
            InitializeComponent();

            // Avoid stealing focus from the control panel when the visualizer opens
            ShowActivated = false;
            Topmost = false;

            // Delay heavy initialization until the window is loaded
            Loaded += VisualizerWindow_Loaded;
            Unloaded += VisualizerWindow_Unloaded;
        }

        private void VisualizerWindow_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                InitializeAudio();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Visualizer audio init failed: {ex.Message}\n\n{ex.StackTrace}", "NekoBeats - Visualizer Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                // Continue without audio if initialization fails
            }

            SetupRendering();
            ThemeManager.ThemeChanged += OnThemeChanged;
        }

        private void VisualizerWindow_Unloaded(object sender, RoutedEventArgs e)
        {
            ThemeManager.ThemeChanged -= OnThemeChanged;
            renderTimer?.Stop();
            audioProcessor?.Dispose();
        }

        private void InitializeAudio()
        {
            // Wrap Audio initialization so exceptions don't crash startup
            audioProcessor = new AudioProcessor();
            audioProcessor.AudioDataUpdated += OnAudioDataUpdated;
        }

        private void OnAudioDataUpdated(float[] frequencies, double[] fft)
        {
            currentFrequencies = frequencies;
            currentFFT = fft;
        }

        private void SetupRendering()
        {
            if (renderTimer != null) return;
            renderTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(33) // ~30 FPS
            };
            renderTimer.Tick += OnRenderFrame;
            renderTimer.Start();
        }

        private void OnThemeChanged(ITheme newTheme)
        {
            InvalidateVisual();
        }

        private void OnRenderFrame(object sender, EventArgs e)
        {
            InvalidateVisual();
        }

        protected override void OnRender(DrawingContext dc)
        {
            dc.DrawRectangle(Brushes.Transparent, null, new Rect(0, 0, ActualWidth, ActualHeight));
            ThemeManager.CurrentTheme?.Render(dc, currentFrequencies, currentFFT);
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try { this.DragMove(); } catch { }
        }

        protected override void OnClosed(EventArgs e)
        {
            renderTimer?.Stop();
            audioProcessor?.Dispose();
            base.OnClosed(e);
        }
    }
}

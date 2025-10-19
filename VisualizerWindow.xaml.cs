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
            InitializeAudio();
            SetupRendering();
            ThemeManager.ThemeChanged += OnThemeChanged;
        }

        private void InitializeAudio()
        {
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
            renderTimer = new DispatcherTimer();
            renderTimer.Interval = TimeSpan.FromMilliseconds(33); // ~30 FPS
            renderTimer.Tick += OnRenderFrame;
            renderTimer.Start();
        }

        private void OnThemeChanged(ITheme newTheme)
        {
            // Force immediate redraw on theme change
            InvalidateVisual();
        }

        private void OnRenderFrame(object sender, EventArgs e)
        {
            // Force redraw with latest audio data
            InvalidateVisual();
        }

        protected override void OnRender(DrawingContext dc)
        {
            // Clear with transparent background
            dc.DrawRectangle(Brushes.Transparent, null, new Rect(0, 0, ActualWidth, ActualHeight));
            
            // Render current theme with real audio data
            ThemeManager.CurrentTheme?.Render(dc, currentFrequencies, currentFFT);
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        protected override void OnClosed(EventArgs e)
        {
            renderTimer?.Stop();
            audioProcessor?.Dispose();
            base.OnClosed(e);
        }
    }
}

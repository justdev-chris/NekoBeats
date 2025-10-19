using System.Windows.Media;

namespace NekoBeats
{
    public class GradientBarsTheme : ITheme
    {
        public string Name => "🌈 Gradient Bars";

        public void Render(DrawingContext dc, float[] frequencies, double[] fft)
        {
            if (frequencies == null || frequencies.Length == 0) return;

            double barWidth = 400.0 / frequencies.Length;
            double maxHeight = 150;

            for (int i = 0; i < frequencies.Length; i++)
            {
                double height = (frequencies[i] / 100.0) * maxHeight;
                double x = i * barWidth;
                double y = 200 - height;

                // Dynamic gradient based on frequency intensity
                var intensity = Math.Min(frequencies[i] / 100.0, 1.0);
                var color1 = Color.FromRgb((byte)(0xff * intensity), 0x6a, 0x00);
                var color2 = Color.FromRgb(0x9c, 0x27, (byte)(0xb0 * intensity));

                var brush = new LinearGradientBrush(color1, color2, 90);
                dc.DrawRectangle(brush, null, new Rect(x, y, barWidth - 1, height));
            }

            // Draw NekoBeats text
            var text = new FormattedText("NekoBeats", 
                System.Globalization.CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight,
                new Typeface("Arial"),
                12, 
                Brushes.White);
            dc.DrawText(text, new Point(10, 10));
        }
    }
}

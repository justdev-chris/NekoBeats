using System;
using System.Windows;
using System.Windows.Media;

namespace NekoBeats
{
    public class GradientBarsTheme : ITheme
    {
        public string Name => "🌈 Gradient Bars";

        public void Render(DrawingContext dc, float[] frequencies, double[] fft)
        {
            if (frequencies == null || frequencies.Length == 0) return;

            double screenWidth = 1920; // Fullscreen width
            double screenHeight = 1080; // Fullscreen height
            double barWidth = screenWidth / frequencies.Length;
            double maxHeight = screenHeight * 0.8;

            for (int i = 0; i < frequencies.Length; i++)
            {
                // Apply sensitivity boost
                double boostedValue = frequencies[i] * 1.5;
                double height = (boostedValue / 100.0) * maxHeight;
                double x = i * barWidth;
                double y = screenHeight - height;

                // Dynamic gradient that pulses with intensity
                var intensity = Math.Min(boostedValue / 100.0, 1.0);
                var color1 = Color.FromRgb((byte)(0xff * intensity), (byte)(0x6a * intensity), 0x00);
                var color2 = Color.FromRgb(0x9c, 0x27, (byte)(0xb0 * intensity));

                var brush = new LinearGradientBrush(color1, color2, 90);
                dc.DrawRectangle(brush, null, new Rect(x, y, barWidth - 1, height));
            }
        }
    }
}

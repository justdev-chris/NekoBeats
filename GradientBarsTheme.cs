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

            double screenWidth = 1920;
            double screenHeight = 1080;
            double barWidth = screenWidth / frequencies.Length;
            double maxHeight = screenHeight * 0.8;

            // Apply sensitivity reduction to 50%
            double sensitivity = 0.5;

            for (int i = 0; i < frequencies.Length; i++)
            {
                // Each bar moves differently based on its frequency band
                double movementFactor = 1.0 + (Math.Sin(i * 0.3) * 0.3); // Unique movement pattern per bar
                
                // Apply sensitivity and individual movement
                double adjustedValue = frequencies[i] * sensitivity * movementFactor;
                double height = (adjustedValue / 100.0) * maxHeight;
                
                // Ensure all bars move (minimum height when there's any audio)
                if (adjustedValue > 5) // Threshold to keep bars visible
                {
                    height = Math.Max(height, maxHeight * 0.1); // Minimum 10% height
                }

                double x = i * barWidth;
                double y = screenHeight - height;

                // Dynamic gradient that pulses with intensity
                var intensity = Math.Min(adjustedValue / 100.0, 1.0);
                var color1 = Color.FromRgb((byte)(0xff * intensity), (byte)(0x6a * intensity), 0x00);
                var color2 = Color.FromRgb(0x9c, 0x27, (byte)(0xb0 * intensity));

                var brush = new LinearGradientBrush(color1, color2, 90);
                dc.DrawRectangle(brush, null, new Rect(x, y, barWidth - 1, height));
            }
        }
    }
}

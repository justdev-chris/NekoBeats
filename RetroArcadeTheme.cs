using System;
using System.Windows;
using System.Windows.Media;

namespace NekoBeats
{
    public class RetroArcadeTheme : ITheme
    {
        public string Name => "🎮 Retro Arcade";

        public void Render(DrawingContext dc, float[] frequencies, double[] fft)
        {
            if (frequencies == null) return;

            double screenWidth = 1920;
            double screenHeight = 1080;
            double barWidth = screenWidth / frequencies.Length;
            double maxHeight = screenHeight * 0.9;

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

                // 8-bit style colors with intensity-based pulsing
                var intensity = Math.Min(adjustedValue / 100.0, 1.0);
                var colors = new[]
                {
                    Color.FromRgb((byte)(255 * intensity), 0, 0), // Red
                    Color.FromRgb(0, 0, (byte)(255 * intensity)), // Blue
                    Color.FromRgb(0, (byte)(255 * intensity), 0), // Green
                    Color.FromRgb((byte)(255 * intensity), (byte)(255 * intensity), 0), // Yellow
                    Color.FromRgb((byte)(255 * intensity), 0, (byte)(255 * intensity)), // Magenta
                    Color.FromRgb(0, (byte)(255 * intensity), (byte)(255 * intensity)), // Cyan
                    Color.FromRgb((byte)(255 * intensity), (byte)(255 * intensity), (byte)(255 * intensity)) // White
                };
                var color = colors[i % colors.Length];

                // Pixelated look with larger blocks for fullscreen
                var brush = new SolidColorBrush(color);
                var pen = new Pen(Brushes.Black, 1);

                // Draw larger pixel blocks for fullscreen
                for (double blockY = y; blockY < screenHeight; blockY += 8)
                {
                    dc.DrawRectangle(brush, pen, new Rect(x, blockY, barWidth - 2, 6));
                }

                // Draw retro scan lines across entire screen (intensity-based)
                if (i % 4 == 0 && intensity > 0.3)
                {
                    var scanBrush = new SolidColorBrush(Color.FromArgb((byte)(100 * intensity), 255, 255, 255));
                    dc.DrawLine(new Pen(scanBrush, 1), 
                        new Point(0, y), new Point(screenWidth, y));
                }
            }

            // Draw pulsing retro game border based on overall audio intensity
            double overallIntensity = 0;
            for (int i = 0; i < Math.Min(frequencies.Length, 10); i++)
            {
                overallIntensity += frequencies[i];
            }
            overallIntensity = Math.Min(overallIntensity / 1000.0, 1.0);
            
            var borderColor = Color.FromRgb((byte)(255 * overallIntensity), (byte)(255 * overallIntensity), 255);
            var borderPen = new Pen(new SolidColorBrush(borderColor), 3 + (overallIntensity * 2));
            dc.DrawRectangle(null, borderPen, new Rect(10, 10, screenWidth - 20, screenHeight - 20));

            // Draw retro "NEKO BEATS" title that pulses
            var pulse = (Math.Sin(DateTime.Now.Millisecond * 0.01) + 1) * 0.5;
            var titleBrush = new SolidColorBrush(Color.FromRgb((byte)(255 * pulse), (byte)(100 * pulse), 255));
            var titleText = new FormattedText("NEKO BEATS", 
                System.Globalization.CultureInfo.CurrentCulture, 
                FlowDirection.LeftToRight,
                new Typeface("Courier New"), 36, titleBrush);
            dc.DrawText(titleText, new Point(screenWidth / 2 - 150, 30));
        }
    }
}

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

            for (int i = 0; i < frequencies.Length; i++)
            {
                // Boost sensitivity
                double boostedValue = frequencies[i] * 1.8;
                double height = (boostedValue / 100.0) * maxHeight;
                double x = i * barWidth;
                double y = screenHeight - height;

                // 8-bit style colors
                var colors = new[]
                {
                    Colors.Red, Colors.Blue, Colors.Green, Colors.Yellow,
                    Colors.Magenta, Colors.Cyan, Colors.White
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

                // Draw retro scan lines across entire screen
                if (i % 4 == 0)
                {
                    dc.DrawLine(new Pen(Brushes.Black, 1), 
                        new Point(0, y), new Point(screenWidth, y));
                }
            }

            // Draw retro game border
            var borderPen = new Pen(Brushes.White, 3);
            dc.DrawRectangle(null, borderPen, new Rect(10, 10, screenWidth - 20, screenHeight - 20));
        }
    }
}

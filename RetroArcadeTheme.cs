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

            double barWidth = 400.0 / frequencies.Length;
            double maxHeight = 150;
            var random = new Random();

            for (int i = 0; i < frequencies.Length; i++)
            {
                double height = (frequencies[i] / 100.0) * maxHeight;
                double x = i * barWidth;
                double y = 200 - height;

                // 8-bit style colors
                var colors = new[]
                {
                    Colors.Red, Colors.Blue, Colors.Green, Colors.Yellow,
                    Colors.Magenta, Colors.Cyan, Colors.White
                };
                var color = colors[i % colors.Length];

                // Pixelated look
                var brush = new SolidColorBrush(color);
                var pen = new Pen(Brushes.Black, 1);

                // Draw pixel blocks
                for (double blockY = y; blockY < 200; blockY += 4)
                {
                    dc.DrawRectangle(brush, pen, new Rect(x, blockY, barWidth - 2, 3));
                }

                // Draw scan lines
                if (i % 2 == 0)
                {
                    dc.DrawLine(new Pen(Brushes.Black, 0.5), 
                        new Point(0, y), new Point(400, y));
                }
            }
        }
    }
}

using System;
using System.Windows.Media;

namespace NekoBeats
{
    public class SpaceNebulaTheme : ITheme
    {
        public string Name => "🌌 Space Nebula";

        public void Render(DrawingContext dc, float[] frequencies, double[] fft)
        {
            if (frequencies == null) return;

            var random = new Random();
            
            // Draw stars
            for (int i = 0; i < 50; i++)
            {
                double x = random.NextDouble() * 400;
                double y = random.NextDouble() * 200;
                double size = random.NextDouble() * 2 + 0.5;
                
                var starBrush = new SolidColorBrush(Colors.White);
                dc.DrawEllipse(starBrush, null, new Point(x, y), size, size);
            }

            // Draw nebula clouds based on frequencies
            for (int i = 0; i < frequencies.Length; i++)
            {
                if (frequencies[i] > 20)
                {
                    double x = (i * 400.0 / frequencies.Length) + random.NextDouble() * 20 - 10;
                    double y = 100 + (random.NextDouble() * 40 - 20);
                    double radius = (frequencies[i] / 100.0) * 30 + 10;

                    var nebulaBrush = new RadialGradientBrush(
                        Color.FromArgb((byte)(frequencies[i] * 2), 0x9d, 0x4e, 0xff),
                        Colors.Transparent);

                    dc.DrawEllipse(nebulaBrush, null, new Point(x, y), radius, radius);
                }
            }

            // Draw planets
            dc.DrawEllipse(
                new RadialGradientBrush(Colors.OrangeRed, Colors.DarkRed),
                null, new Point(350, 50), 15, 15);
            
            dc.DrawEllipse(
                new RadialGradientBrush(Colors.LightBlue, Colors.Blue),
                null, new Point(50, 150), 20, 20);
        }
    }
}

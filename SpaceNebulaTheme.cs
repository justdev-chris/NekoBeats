using System;
using System.Windows;
using System.Windows.Media;
using System.Collections.Generic;

namespace NekoBeats
{
    public class SpaceNebulaTheme : ITheme
    {
        public string Name => "🌌 Space Nebula";
        private List<NebulaCloud> clouds = new List<NebulaCloud>();
        private Random random = new Random();
        private DateTime lastBeatTime = DateTime.Now;

        public class NebulaCloud
        {
            public Point Position { get; set; }
            public double Size { get; set; }
            public double Life { get; set; } = 1.0;
            public Color Color { get; set; }
        }

        public void Render(DrawingContext dc, float[] frequencies, double[] fft)
        {
            if (frequencies == null) return;

            double screenWidth = 1920;
            double screenHeight = 1080;

            // Draw background stars
            for (int i = 0; i < 200; i++)
            {
                double x = random.NextDouble() * screenWidth;
                double y = random.NextDouble() * screenHeight;
                double size = random.NextDouble() * 3 + 0.5;
                double twinkle = (Math.Sin(DateTime.Now.Millisecond * 0.001 + i) + 1) * 0.5;
                
                var starBrush = new SolidColorBrush(Color.FromArgb(
                    (byte)(twinkle * 255), 0xff, 0xff, 0xff));
                dc.DrawEllipse(starBrush, null, new Point(x, y), size, size);
            }

            // Detect beats and create new nebula clouds
            bool hasBeat = frequencies.Length > 0 && frequencies[0] > 50;

            if (hasBeat && (DateTime.Now - lastBeatTime).TotalMilliseconds > 300)
            {
                lastBeatTime = DateTime.Now;
                
                // Create new nebula cloud on beat
                clouds.Add(new NebulaCloud
                {
                    Position = new Point(
                        random.NextDouble() * screenWidth,
                        random.NextDouble() * screenHeight),
                    Size = 50 + random.NextDouble() * 100,
                    Color = Color.FromRgb(
                        (byte)random.Next(100, 255),
                        (byte)random.Next(50, 150),
                        (byte)random.Next(150, 255))
                });
            }

            // Update and render nebula clouds
            for (int i = clouds.Count - 1; i >= 0; i--)
            {
                var cloud = clouds[i];
                cloud.Life -= 0.01;
                cloud.Size += 2; // Expand over time

                if (cloud.Life <= 0)
                {
                    clouds.RemoveAt(i);
                    continue;
                }

                var nebulaBrush = new RadialGradientBrush(
                    Color.FromArgb((byte)(cloud.Life * 150), cloud.Color.R, cloud.Color.G, cloud.Color.B),
                    Colors.Transparent);

                dc.DrawEllipse(nebulaBrush, null, cloud.Position, cloud.Size, cloud.Size);
            }

            // Draw central black hole that pulses with bass
            var bassIntensity = frequencies.Length > 0 ? frequencies[0] / 100.0 : 0.3;
            var pulse = (Math.Sin(DateTime.Now.Millisecond * 0.005) + 1) * 0.2 + bassIntensity * 0.8;

            var blackHoleBrush = new RadialGradientBrush(
                Color.FromRgb(0x00, 0x00, 0x00),
                Color.FromArgb(0, 0x9d, 0x4e, 0xff));

            dc.DrawEllipse(blackHoleBrush, null,
                new Point(screenWidth / 2, screenHeight / 2),
                80 + pulse * 40, 80 + pulse * 40);
        }
    }
}

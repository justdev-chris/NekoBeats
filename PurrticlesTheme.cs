using System;
using System.Windows.Media;

namespace NekoBeats
{
    public class PurrticlesTheme : ITheme
    {
        public string Name => "🐾 Purr-ticles";

        public void Render(DrawingContext dc, float[] frequencies, double[] fft)
        {
            if (frequencies == null) return;

            var random = new Random();
            double centerX = 200;
            double centerY = 100;

            // Draw pulsing center orb (cat face)
            var pulse = (Math.Sin(DateTime.Now.Millisecond * 0.01) + 1) * 0.5;
            var orbBrush = new RadialGradientBrush(
                Color.FromRgb(0xff, 0x9a, 0x00),
                Color.FromRgb(0xff, 0x6a, 0x00));
            dc.DrawEllipse(orbBrush, null, new Point(centerX, centerY), 30 + pulse * 10, 30 + pulse * 10);

            // Draw particle effects based on frequencies
            for (int i = 0; i < frequencies.Length; i++)
            {
                if (frequencies[i] > 30)
                {
                    double angle = (i * 11.25) * Math.PI / 180;
                    double distance = 50 + (frequencies[i] / 100.0) * 50;
                    double x = centerX + Math.Cos(angle) * distance;
                    double y = centerY + Math.Sin(angle) * distance;

                    var particleBrush = new SolidColorBrush(Color.FromArgb(
                        (byte)(frequencies[i] * 2.5),
                        0xff, 0xff, 0xff));

                    dc.DrawEllipse(particleBrush, null, new Point(x, y), 3, 3);
                }
            }
        }
    }
}

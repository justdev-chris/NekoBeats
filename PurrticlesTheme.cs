using System;
using System.Windows;
using System.Windows.Media;
using System.Collections.Generic;

namespace NekoBeats
{
    public class PurrticlesTheme : ITheme
    {
        public string Name => "🐾 Purr-ticles";
        private List<Particle> particles = new List<Particle>();
        private Random random = new Random();
        private DateTime lastBeatTime = DateTime.Now;

        public class Particle
        {
            public Point Position { get; set; }
            public Vector Velocity { get; set; }
            public double Life { get; set; } = 1.0;
            public Color Color { get; set; }
        }

        public void Render(DrawingContext dc, float[] frequencies, double[] fft)
        {
            if (frequencies == null) return;

            double screenWidth = 1920;
            double screenHeight = 1080;

            // Detect beats (when bass frequencies spike)
            bool hasBeat = frequencies.Length > 0 && frequencies[0] > 60; // Bass in first few bands

            if (hasBeat && (DateTime.Now - lastBeatTime).TotalMilliseconds > 200)
            {
                lastBeatTime = DateTime.Now;
                
                // Spawn particles on beat
                for (int i = 0; i < 20; i++)
                {
                    particles.Add(new Particle
                    {
                        Position = new Point(screenWidth / 2, screenHeight / 2),
                        Velocity = new Vector(
                            (random.NextDouble() - 0.5) * 10,
                            (random.NextDouble() - 0.5) * 10),
                        Color = Color.FromRgb(
                            (byte)random.Next(200, 255),
                            (byte)random.Next(100, 200),
                            (byte)random.Next(100, 255))
                    });
                }
            }

            // Update and render particles
            for (int i = particles.Count - 1; i >= 0; i--)
            {
                var particle = particles[i];
                particle.Position = new Point(
                    particle.Position.X + particle.Velocity.X,
                    particle.Position.Y + particle.Velocity.Y);
                particle.Life -= 0.02;

                if (particle.Life <= 0)
                {
                    particles.RemoveAt(i);
                    continue;
                }

                var brush = new SolidColorBrush(Color.FromArgb(
                    (byte)(particle.Life * 255), 
                    particle.Color.R, particle.Color.G, particle.Color.B));

                dc.DrawEllipse(brush, null, particle.Position, 5 * particle.Life, 5 * particle.Life);
            }

            // Draw central cat face that pulses with audio
            var centerIntensity = frequencies.Length > 0 ? frequencies[0] / 100.0 : 0.5;
            var pulse = (Math.Sin(DateTime.Now.Millisecond * 0.01) + 1) * 0.3 + centerIntensity * 0.7;
            
            var orbBrush = new RadialGradientBrush(
                Color.FromRgb(0xff, (byte)(0x9a * pulse), 0x00),
                Color.FromRgb((byte)(0xff * pulse), 0x6a, 0x00));
            
            dc.DrawEllipse(orbBrush, null, 
                new Point(screenWidth / 2, screenHeight / 2), 
                50 + pulse * 30, 50 + pulse * 30);
        }
    }
}

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
        private int beatCounter = 0;

        public class Particle
        {
            public Point Position { get; set; }
            public Vector Velocity { get; set; }
            public double Life { get; set; } = 1.0;
            public Color Color { get; set; }
            public double Size { get; set; } = 5.0;
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
                beatCounter++;
                
                // Spawn NEW particles on each beat with different patterns
                int particlesToSpawn = 15 + (beatCounter % 3) * 5; // Varies each beat
                
                for (int i = 0; i < particlesToSpawn; i++)
                {
                    double angle = (i * 360.0 / particlesToSpawn) + (beatCounter * 15);
                    double speed = 3 + (random.NextDouble() * 4);
                    
                    particles.Add(new Particle
                    {
                        Position = new Point(screenWidth / 2, screenHeight / 2),
                        Velocity = new Vector(
                            Math.Cos(angle * Math.PI / 180) * speed,
                            Math.Sin(angle * Math.PI / 180) * speed),
                        Color = Color.FromRgb(
                            (byte)random.Next(150, 255),
                            (byte)random.Next(100, 200), 
                            (byte)random.Next(150, 255)),
                        Size = 3 + random.NextDouble() * 4,
                        Life = 0.8 + (random.NextDouble() * 0.4)
                    });
                }

                // Special burst every 4 beats
                if (beatCounter % 4 == 0)
                {
                    for (int i = 0; i < 10; i++)
                    {
                        particles.Add(new Particle
                        {
                            Position = new Point(
                                screenWidth / 2 + (random.NextDouble() - 0.5) * 100,
                                screenHeight / 2 + (random.NextDouble() - 0.5) * 100),
                            Velocity = new Vector(
                                (random.NextDouble() - 0.5) * 8,
                                (random.NextDouble() - 0.5) * 8),
                            Color = Colors.Gold,
                            Size = 6,
                            Life = 1.2
                        });
                    }
                }
            }

            // Update and render particles
            for (int i = particles.Count - 1; i >= 0; i--)
            {
                var particle = particles[i];
                particle.Position = new Point(
                    particle.Position.X + particle.Velocity.X,
                    particle.Position.Y + particle.Velocity.Y);
                particle.Life -= 0.016; // Slower fade

                // Add some gravity to particles
                particle.Velocity = new Vector(
                    particle.Velocity.X * 0.98,
                    particle.Velocity.Y + 0.1);

                if (particle.Life <= 0)
                {
                    particles.RemoveAt(i);
                    continue;
                }

                var brush = new SolidColorBrush(Color.FromArgb(
                    (byte)(particle.Life * 255), 
                    particle.Color.R, particle.Color.G, particle.Color.B));

                dc.DrawEllipse(brush, null, particle.Position, 
                    particle.Size * particle.Life, particle.Size * particle.Life);
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

            // Draw beat counter (for debugging)
            var text = new FormattedText($"Beats: {beatCounter}", 
                System.Globalization.CultureInfo.CurrentCulture, 
                FlowDirection.LeftToRight,
                new Typeface("Arial"), 20, Brushes.White);
            dc.DrawText(text, new Point(20, 20));
        }
    }
}

using System.Windows.Media;

namespace NekoBeats
{
    public interface ITheme
    {
        string Name { get; }
        void Render(DrawingContext dc, float[] frequencies, double[] fft);
    }
}

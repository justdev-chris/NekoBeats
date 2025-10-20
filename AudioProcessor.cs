using System;
using NAudio.Wave;
using NAudio.Dsp;

namespace NekoBeats
{
    public class AudioProcessor : IDisposable
    {
        private WasapiLoopbackCapture capture;
        private readonly int fftLength = 1024;
        private Complex[] fftBuffer;
        private float[] fftResults;
        public event Action<float[], double[]> AudioDataUpdated;

        public AudioProcessor()
        {
            InitializeAudioCapture();
        }

        private void InitializeAudioCapture()
        {
            try
            {
                capture = new WasapiLoopbackCapture();
                fftBuffer = new Complex[fftLength];
                fftResults = new float[fftLength / 2];

                capture.DataAvailable += OnDataAvailable;
                capture.StartRecording();
            }
            catch (Exception ex)
            {
                // Fallback to mock data if audio capture fails
                StartMockData();
            }
        }

        private void OnDataAvailable(object sender, WaveInEventArgs e)
        {
            if (e.BytesRecorded == 0) return;

            // Convert byte data to float for FFT
            var buffer = new float[e.BytesRecorded / 4];
            Buffer.BlockCopy(e.Buffer, 0, buffer, 0, e.BytesRecorded);

            // Apply FFT
            ApplyFFT(buffer);

            // Convert to frequency bands
            var frequencyBands = ConvertToFrequencyBands();

            // Notify subscribers
double[] fftResultsDouble = Array.ConvertAll(fftResults, x => (double)x);
AudioDataUpdated?.Invoke(frequencyBands, fftResultsDouble);
        }

        private void ApplyFFT(float[] buffer)
        {
            // Fill FFT buffer
            int samplesToCopy = Math.Min(fftLength, buffer.Length);
            for (int i = 0; i < samplesToCopy; i++)
            {
                fftBuffer[i].X = (float)(buffer[i] * FastFourierTransform.HammingWindow(i, fftLength));
                fftBuffer[i].Y = 0;
            }

            // Fill remaining with zeros if needed
            for (int i = samplesToCopy; i < fftLength; i++)
            {
                fftBuffer[i].X = 0;
                fftBuffer[i].Y = 0;
            }

            // Perform FFT
            FastFourierTransform.FFT(true, (int)Math.Log(fftLength, 2.0), fftBuffer);

            // Convert to magnitude
            for (int i = 0; i < fftLength / 2; i++)
            {
                float magnitude = (float)Math.Sqrt(fftBuffer[i].X * fftBuffer[i].X + fftBuffer[i].Y * fftBuffer[i].Y);
                fftResults[i] = magnitude;
            }
        }

    private float[] ConvertToFrequencyBands()
    {
        var bands = new float[64]; // More bands for better resolution
        int bandsPerGroup = fftResults.Length / bands.Length;

        for (int i = 0; i < bands.Length; i++)
        {
            float sum = 0;
            for (int j = 0; j < bandsPerGroup; j++)
            {
            int index = i * bandsPerGroup + j;
            if (index < fftResults.Length)
            {
                sum += fftResults[index];
            }
        }
        bands[i] = sum / bandsPerGroup * 5000f; // Increased sensitivity
        bands[i] = Math.Min(bands[i], 100f);
    }

    return bands;
}
        private void StartMockData()
        {
            var random = new Random();
            var mockTimer = new System.Timers.Timer(100);
            mockTimer.Elapsed += (s, e) =>
            {
                var mockBands = new float[32];
                for (int i = 0; i < mockBands.Length; i++)
                {
                    mockBands[i] = (float)(random.NextDouble() * 80 + 20);
                }
                AudioDataUpdated?.Invoke(mockBands, new double[0]);
            };
            mockTimer.Start();
        }

        public void Dispose()
        {
            capture?.StopRecording();
            capture?.Dispose();
        }
    }
}

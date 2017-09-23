using NAudio.Wave;

namespace AudioPPM
{
    /// <inheritdoc />
    /// <summary>
    /// Wrapper to simplify create provider with array of floats
    /// </summary>
    public abstract class WaveProvider32 : IWaveProvider
    {
        private WaveFormat _waveFormat;
        public WaveFormat WaveFormat => _waveFormat;

        protected WaveProvider32() : this(44100, 1)
        {
        }

        protected WaveProvider32(int sampleRate, int channels)
        {
            SetWaveFormat(sampleRate, channels);
        }

        public void SetWaveFormat(int sampleRate, int channels)
        {
            this._waveFormat = WaveFormat.CreateIeeeFloatWaveFormat(sampleRate, channels);
        }

        public int Read(byte[] buffer, int offset, int count)
        {
            WaveBuffer waveBuffer = new WaveBuffer(buffer);
            int samplesRequired = count / 4;
            int samplesRead = Read(waveBuffer.FloatBuffer, offset / 4, samplesRequired);
            return samplesRead * 4;
        }

        public abstract int Read(float[] buffer, int offset, int sampleCount);
    }
}
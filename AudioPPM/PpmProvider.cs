using System;
using NAudio.Wave;

namespace AudioPPM
{
    /// <inheritdoc />
    /// <summary>
    /// IProvider implementation that generates PPM controlling singal and change
    /// controlling values
    /// </summary>
    public class PpmProvider : WaveProvider32
    {
        private readonly WaveFormat _waveFormat;
        private PpmProfile _ppmProfile;

        // Data buffers
        private readonly byte[] _playingBuffer;         // Currently playing values
        private readonly byte[] _writingBuffer;         // User write new values there
        private object _lock;

        // PPM timing params
        private int _currentChannel;
        private int _currentChannelSample;
        private int _pauseSamples;
        private int _periodSamples;
        private int _totalSamples;

        private const float DIVIDER = 1000000;          // microseconds to seconds

        /// <summary>
        /// PPM timings and polarity settings. Read only.
        /// </summary>
        public PpmProfile PpmProfile => _ppmProfile;

        /// <summary>
        /// Number of channels
        /// </summary>
        public byte ChannelsCount { get; private set; }



        public PpmProvider(byte channels, PpmProfile ppmProfile) : this(channels, ppmProfile, 44100)
        {
        }


        public PpmProvider(byte channels, PpmProfile ppmProfile, int sampleRate)
        {
            ChannelsCount = channels;
            _ppmProfile = ppmProfile;
            _waveFormat = new WaveFormat(sampleRate, 1);

            _playingBuffer = new byte[channels];
            _writingBuffer = new byte[channels];

            _lock = new object();

            _currentChannel = 0;
            _currentChannelSample = 0;
            _totalSamples = 0;

            _pauseSamples = (int)(_ppmProfile.PauseDuration / 1000000.0 * _waveFormat.SampleRate);

            TakeNextChannel();
        }


        /// <summary>
        /// Set new values. They will be applied from new cycle. 
        /// NOTE: Number of values should be equal to number of channels.
        /// Otherwise exception will be thrown.
        /// </summary>
        /// <param name="values"></param>
        public void SetValues(byte[] values)
        {
            // 1 - _currentBuffer is buffer swapping
            lock (_lock)
            {
                Array.Copy(values, _writingBuffer, ChannelsCount);
            }
        }

        public override int Read(float[] buffer, int offset, int sampleCount)
        {
            for (int i = offset; i < sampleCount; i++)
            {
                if (_currentChannelSample <= _pauseSamples)
                    buffer[i] = GetValue(false);
                else
                    buffer[i] = GetValue(true);

                _currentChannelSample++;
                _totalSamples--;

                if (_currentChannelSample > _periodSamples)
                    TakeNextChannel();
            }


            return sampleCount;
        }


        /// <summary>
        /// Get value from next channel and calculate timings
        /// </summary>
        private void TakeNextChannel()
        {
            _currentChannel++;
            _currentChannelSample = 0;

            if (_currentChannel == ChannelsCount)
            {
                //Pause
                _periodSamples = _totalSamples;

                _totalSamples = 0;
                _currentChannel = -1;

                lock (_lock)
                {
                    Array.Copy(_writingBuffer, _playingBuffer, ChannelsCount);
                }
            }
            else
            {
                float period = _playingBuffer[_currentChannel] / 255.0f *
                            (_ppmProfile.MaxChannelDuration -
                            _ppmProfile.MinChannelDuration) +
                            _ppmProfile.MinChannelDuration;

                _periodSamples = (int)(period / DIVIDER * _waveFormat.SampleRate);

                // New packages started - reset total samples counter
                if (_currentChannel == 0)
                    _totalSamples = (int)(_ppmProfile.Period / DIVIDER * _waveFormat.SampleRate);
            }
        }

        /// <summary>
        /// Get sample value by selected PPM polarity and needed value
        /// </summary>
        /// <param name="isSignal">Is value signal (variable length) or pause (constant length)</param>
        /// <returns></returns>
        private float GetValue(bool isSignal)
        {
            if (isSignal)
                return _ppmProfile.Polarity == PpmPolarity.HIGH ? 0 : -1;
            else
                return _ppmProfile.Polarity == PpmPolarity.HIGH ? -1 : 0;
        }
    }
}
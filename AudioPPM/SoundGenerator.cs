using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.CoreAudioApi;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System.Diagnostics;

namespace AudioPPM
{
    public class PpmGenerator
    {
        private IWavePlayer _player;
        private PpmProvider _provider;

        public PpmGenerator(byte channelsCount, MMDevice device)
        {
            _provider = new PpmProvider(channelsCount);
            _player = new  WasapiOut(device, AudioClientShareMode.Shared, false, 30);
            _player.Init(_provider);
        }

        public static MMDeviceCollection GetDevices()
        {
            MMDeviceEnumerator enumerator = new MMDeviceEnumerator();
            return enumerator.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.All);
        }

        public void SetValues(byte[] values)
        {
            _provider.SetValues(values);
        }

        public void Start()
        {
            _player.Play();

        }

        public void Stop()
        {
            _player.Stop();
        }
    }

    /// <summary>
    /// Polarity of the various length part of PPM signal
    /// </summary>
    public enum PpmPolarity
    {
        HIGH,
        LOW
    }

    /// <summary>
    /// Set of PPM settings (timings and polarity)
    /// </summary>
    public struct PpmProfile
    {
        /// <summary>
        /// Duration of the pause
        /// (part of PPM cycle which duration is constant)
        /// </summary>
        public int PauseDuration { get; set; }

        /// <summary>
        /// Polarity of the various length part of PPM signal
        /// </summary>
        public PpmPolarity Polarity { get; set; }

        /// <summary>
        /// Minimum period duration (mks)
        /// </summary>
        public int MinChannelDuration { get; set; }

        /// <summary>
        /// Maximum period duration (mks)
        /// </summary>
        public int MaxChannelDuration { get; set; }

        /// <summary>
        /// Whole period (all channels + sync idle) (mks)
        /// </summary>
        public int Period { get; set; }

        public static PpmProfile Default()
        {
            return new PpmProfile()
            {
                PauseDuration = 500,
                Polarity = PpmPolarity.HIGH,
                MinChannelDuration = 1000,
                MaxChannelDuration = 2000,
                Period = 20000
            };
        }
    }


    public abstract class WaveProvider32 : IWaveProvider
    {
        private WaveFormat _waveFormat;
        public WaveFormat WaveFormat => _waveFormat;

        public WaveProvider32()
            : this(44100, 1)
        {
        }

        public WaveProvider32(int sampleRate, int channels)
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

    /// <summary>
    /// Generate PPM signal
    /// </summary>
    public class PpmProvider : WaveProvider32
    {
        private readonly WaveFormat _waveFormat;
        private PpmProfile _ppmProfile;

        // Data buffers
        private readonly byte[] _playingBuffer;
        private readonly byte[] _writingBuffer;
        private int _currentChannel;
        private object _lock;

        // PPM timing params
        private int _currentChannelSample;
        private int _pauseSamples;
        private int _periodSamples;
        private int _totalSamples;

        private const float DIVIDER = 1000000;

        /// <summary>
        /// PPM timings and polarity settings. Read only.
        /// </summary>
        public PpmProfile PpmProfile => _ppmProfile;

        public byte ChannelsCount { get; private set; }



        public PpmProvider(byte channels) : this(channels, 44100, PpmProfile.Default())
        {
        }


        public PpmProvider(byte channels, int sampleRate, PpmProfile ppmProfile)
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
                    buffer[i] = 1;// GetValue(false);
                else
                    buffer[i] = 0;// GetValue(true);

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

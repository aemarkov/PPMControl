using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.CoreAudioApi;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;

namespace AudioPPM
{
    public class SoundGenerator
    {
        private IWavePlayer _player;

        public SoundGenerator()
        {
            var provider = new PpmProvider(6);

            _player = new DirectSoundOut();
            _player.Init(provider);

            /*byte[] a = new byte[10];

            while (true)
            {
                provider.Read(a, 0, 10);
            }*/
        }

        public void Play()
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
                Polarity = PpmPolarity.LOW,
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

        // Data buffers. Double buffering is used
        private readonly byte[][] _buffers;
        private readonly int _currentBuffer;
        private int _currentChannel;

        // PPM timing params
        private int _currentChannelSample;
        private int _pauseSamples;
        private int _periodSamples;
        private int _totalSamples;

        public WaveFormat WaveFormat => _waveFormat;

        /// <summary>
        /// PPM timings and polarity settings. Read only.
        /// </summary>
        public PpmProfile PpmProfile => _ppmProfile;

        public byte Channels { get; private set; }



        public PpmProvider(byte channels) : this(channels, 44100, PpmProfile.Default())
        {
        }


        public PpmProvider(byte channels, int sampleRate, PpmProfile ppmProfile)
        {
            Channels = channels;
            _ppmProfile = ppmProfile;
            _waveFormat = new WaveFormat(sampleRate, 1);

            _buffers = new byte[2][];
            for (int i = 0; i < 2; i++)
                _buffers[i] = new byte[] { 255, 0, 0, 255, 0, 255 };

            _currentBuffer = 0;
            _currentChannel = 0;
            _currentChannelSample = 0;
            _totalSamples = 0;

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
            Array.Copy(values, _buffers[1 - _currentBuffer], Channels);
        }

        public override int Read(float[] buffer, int offset, int sampleCount)
        {
            for (int i = offset; i < sampleCount; i++)
            {
                //byte polarity = _ppmProfile.Polarity == PpmPolarity.LOW ? (byte) 0 : (byte)1;
                if (_currentChannelSample <= _pauseSamples)
                    buffer[i] = -1f; //(byte) (1 - polarity);
                else
                    buffer[i] = 0; //polarity;

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
            Console.WriteLine();

            _currentChannel++;
            _currentChannelSample = 0;

            if (_currentChannel == Channels)
            {
                //Pause

                _periodSamples = _totalSamples;
                _pauseSamples = 0;

                _totalSamples = 0;
                _currentChannel = -1;
            }
            else
            {
                _pauseSamples = (int) (_ppmProfile.PauseDuration / 1000000.0 * _waveFormat.SampleRate);

                double period = _buffers[_currentBuffer][_currentChannel] / 255.0 *
                                                                              (_ppmProfile.MaxChannelDuration -
                                                                               _ppmProfile.MinChannelDuration) +
                                                                              _ppmProfile.MinChannelDuration;

                _periodSamples = (int) (period / 1000000.0 * _waveFormat.SampleRate);

                // New packages started - reset total samples counter
                if (_currentChannel == 0)
                    _totalSamples = (int) (_ppmProfile.Period / 1000000.0 * _waveFormat.SampleRate);
            }
        }
    }
}

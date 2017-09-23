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
    /// <summary>
    /// Generate PPM controlling singal to selected output device using WASAPI
    /// </summary>
    public class PpmGenerator
    {
        private readonly IWavePlayer _player;
        private readonly PpmProvider _provider;

        /// <summary>
        /// Create new PpmGenerator object
        /// </summary>
        /// <param name="channelsCount">Number of channels in signal</param>
        /// <param name="ppmProfile">PPM profile for using transmitter</param>
        /// <param name="device">Selected output device</param>
        public PpmGenerator(byte channelsCount, PpmProfile ppmProfile, MMDevice device)
        {
            _provider = new PpmProvider(channelsCount, ppmProfile);
            _player = new  WasapiOut(device, AudioClientShareMode.Shared, false, 30);
            _player.Init(_provider);
        }

        /// <summary>
        /// Return list of avaliable to output devises
        /// </summary>
        /// <returns></returns>
        public static MMDeviceCollection GetDevices()
        {
            MMDeviceEnumerator enumerator = new MMDeviceEnumerator();
            return enumerator.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active);
        }


        /// <summary>
        /// Set new values to the signal.
        /// Number of values must be same as number of channels
        /// </summary>
        /// <param name="values">New value for every channel</param>
        public void SetValues(byte[] values)
        {
            _provider.SetValues(values);
        }


        /// <summary>
        /// Begin playing signal
        /// </summary>
        public void Start()
        {
            _player.Play();

        }


        /// <summary>
        /// Stop playing signal
        /// </summary>
        public void Stop()
        {
            _player.Stop();
        }
    }

}

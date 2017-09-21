using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AudioPPM;
using NAudio.CoreAudioApi;

namespace ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var gen = new SoundGenerator();

            MMDeviceEnumerator enumerator = new MMDeviceEnumerator();
            foreach (MMDevice device in enumerator.EnumerateAudioEndPoints(DataFlow.Capture, DeviceState.All))
            {
               // Console.WriteLine("{0}, {1}", device.FriendlyName, device.State);
            }

            gen.Play();
            Console.ReadKey();
            gen.Stop();
        }
    }
}

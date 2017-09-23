using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AudioPPM;
using System.Diagnostics;
using NAudio.CoreAudioApi;

namespace ControlGUI
{
    public partial class Form1 : Form
    {
        PpmGenerator _generator;

        private const int CHANNELS_COUNT = 6;
        readonly byte[] _channelValues;
        IList<MMDevice> _devices;
        private bool _isPlaying;

        public Form1()
        {
            InitializeComponent();

            SetPlaying(false);
            _channelValues = new byte[CHANNELS_COUNT];
            UpdateDevices();

            this.FormClosing += Form1_FormClosing;
            trackbarThrottle.ValueChanged += Trackbar_ValueChanged;
            trackbarRudder.ValueChanged += Trackbar_ValueChanged;
            trackbarElevator.ValueChanged += Trackbar_ValueChanged;
            trackbarEleron.ValueChanged += Trackbar_ValueChanged;
        }

        #region GUI EVENTS

        private void Trackbar_ValueChanged(object sender, EventArgs e)
        {
            UpdateValues();
        }



        // Start PPM Generator
        private void btnStart_Click(object sender, EventArgs e)
        {
            if (_isPlaying)
                return;

            if(listboxDevices.SelectedIndex == -1)
            {
                MessageBox.Show("Please, select the output device", "PPM Control", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _generator = new PpmGenerator(CHANNELS_COUNT, StandartProfiles.FlySky, _devices[listboxDevices.SelectedIndex]);
            _generator.SetValues(_channelValues);
            _generator.Start();
            SetPlaying(true);
        }


        // Stop PPM Generator
        private void btnStop_Click(object sender, EventArgs e)
        {
            _generator?.Stop();
            SetPlaying(false);
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            UpdateDevices();
        }


        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            _generator?.Stop();
        }

        
        #endregion


        // Get values from trackbars and set them to PPM Generator
        private void UpdateValues()
        {
            _channelValues[0] = (byte)trackbarEleron.Value;
            _channelValues[1] = (byte)trackbarElevator.Value;
            _channelValues[2] = (byte)trackbarThrottle.Value;
            _channelValues[3] = (byte)trackbarRudder.Value;

            _generator.SetValues(_channelValues);
        }


        // Update list of output devices
        private void UpdateDevices()
        {
            listboxDevices.Items.Clear();
            _devices = PpmGenerator.GetDevices().OrderBy(x=>x.FriendlyName).ToList();

            foreach (var device in _devices)
                listboxDevices.Items.Add($"{device.FriendlyName} - {device.State}");
        }


        // Set playing state and GUI
        private void SetPlaying(bool isPlaying)
        {
            _isPlaying = isPlaying;

            if (isPlaying)
            {
                btnStart.Enabled = false;
                btnStop.Enabled = true;
            }
            else
            {
                btnStart.Enabled = true;
                btnStop.Enabled = false;
            }
        }
    }
}

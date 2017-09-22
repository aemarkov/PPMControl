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
        byte[] _channelValues;
        MMDeviceCollection _devices;

        public Form1()
        {
            InitializeComponent();

            _channelValues = new byte[CHANNELS_COUNT];
            UpdateDevices();

            this.FormClosing += Form1_FormClosing;
            trackbarThrottle.ValueChanged += Trackbar_ValueChanged;
            trackbarRudder.ValueChanged += Trackbar_ValueChanged;
            trackbarElevator.ValueChanged += Trackbar_ValueChanged;
            trackbarEleron.ValueChanged += Trackbar_ValueChanged;
        }

        private void Trackbar_ValueChanged(object sender, EventArgs e)
        {
            UpdateValues();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_generator != null)
                _generator.Stop();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if(listboxDevices.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите устройство", "PPMControl", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _generator = new PpmGenerator(CHANNELS_COUNT, _devices[listboxDevices.SelectedIndex]);
            UpdateValues();
            _generator.Start();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            if(_generator!=null)
                _generator.Stop();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            UpdateDevices();
        }

        private void UpdateValues()
        {
            _channelValues[0] = (byte)trackbarEleron.Value;
            _channelValues[1] = (byte)trackbarElevator.Value;
            _channelValues[2] = (byte)trackbarThrottle.Value;
            _channelValues[3] = (byte)trackbarRudder.Value;

            _generator.SetValues(_channelValues);
        }

        private void UpdateDevices()
        {
            listboxDevices.Items.Clear();
            _devices = PpmGenerator.GetDevices();
            foreach(var device in _devices)
            {
                listboxDevices.Items.Add($"{device.FriendlyName} - {device.State}");
            }
        }
    }
}

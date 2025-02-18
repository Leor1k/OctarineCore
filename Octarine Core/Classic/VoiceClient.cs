using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows;
using NAudio.Wave;

namespace Octarine_Core.Classic
{
    public class VoiceClient
    {
        private UdpClient _udpClient;
        private string _serverIp = "147.45.175.135";
        private int _port = 5005;
        private IPEndPoint _serverEndPoint;
        private WaveInEvent _waveIn;

        public VoiceClient()
        {
            _udpClient = new UdpClient();
            _serverEndPoint = new IPEndPoint(IPAddress.Parse(_serverIp), _port);

            _waveIn = new WaveInEvent();
            _waveIn.WaveFormat = new WaveFormat(8000, 16, 1); // 8kHz, 16 бит, моно
            _waveIn.DataAvailable += OnAudioData;
        }
       
        private async void OnAudioData(object sender, WaveInEventArgs e)
        {
            await Task.Run(() =>
            {
                _udpClient.Send(e.Buffer, e.Buffer.Length, _serverEndPoint);
            });
        }


        public void StartRecording()
        {
            //MessageBox.Show("Началась трансляция");
            _waveIn.StartRecording();
        }

        public void StopRecording()
        {
            _waveIn.StopRecording();
        }
    }
}

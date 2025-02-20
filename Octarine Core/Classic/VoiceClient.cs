using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using NAudio.Wave;

namespace Octarine_Core.Classic
{
    public class VoiceClient
    {
        private UdpClient _udpClient;
        private string _serverIp = "147.45.175.135"; // IP сервера
        private int _port = 5005;
        private IPEndPoint _serverEndPoint;
        private WaveInEvent _waveIn;
        private Log l = new Log();

        public VoiceClient()
        {
            _udpClient = new UdpClient();
            _serverEndPoint = new IPEndPoint(IPAddress.Parse(_serverIp), _port);
            _udpClient.Client.SendBufferSize = 65536; // 64 KB
            _waveIn = new WaveInEvent
            {
                WaveFormat = new WaveFormat(8000, 16, 1) // 8kHz, 16 бит, моно
            };
            _waveIn.DataAvailable += OnAudioData;
        }

        private async void OnAudioData(object sender, WaveInEventArgs e)
        {
            l.log($"[VoiceClient] Записано {e.BytesRecorded} байт аудиоданных.");
            if (e.BytesRecorded > 0)
            {
                await Task.Run(() => _udpClient.Send(e.Buffer, e.Buffer.Length, _serverEndPoint));
                l.log("[VoiceClient] Отправка аудиоданных...");
            }
        }

        public void StartRecording()
        {
            l.log("[VoiceClient] Начата запись голоса.");
            _waveIn.StartRecording();
        }

        public void StopRecording()
        {
            _waveIn.StopRecording();
        }
    }
}

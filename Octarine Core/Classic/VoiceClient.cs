using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;

namespace Octarine_Core.Classic
{
    public class VoiceClient
    {
        private UdpClient _udpClient;
        private string _serverIp = "147.45.175.135"; // IP сервера
        private int _serverPort = 5005; // Серверный порт
        private IPEndPoint _serverEndPoint;
        private WaveInEvent _waveIn;
        private Log l = new Log();

        public VoiceClient()
        {
            _udpClient = new UdpClient(0); // Используем динамический порт
            _serverEndPoint = new IPEndPoint(IPAddress.Parse(_serverIp), _serverPort);
            _udpClient.Client.SendBufferSize = 65536; // 64 KB

            var localEndPoint = (IPEndPoint)_udpClient.Client.LocalEndPoint;
            l.log($"[VoiceClient] Клиент запущен на порту {localEndPoint.Port}");

            _waveIn = new WaveInEvent
            {
                WaveFormat = new WaveFormat(8000, 16, 1) // 8kHz, 16 бит, моно
            };
            _waveIn.DataAvailable += OnAudioData;
        }

        private async void OnAudioData(object sender, WaveInEventArgs e)
        {
            if (e.BytesRecorded > 0)
            {
                await _udpClient.SendAsync(e.Buffer, e.Buffer.Length, _serverEndPoint);
                l.log($"[VoiceClient] Отправлено {e.BytesRecorded} байт на {_serverEndPoint}");
            }
        }

        public async Task PunchHole()
        {
            try
            {
                byte[] dummyData = Encoding.UTF8.GetBytes("NAT_HOLE");
                await _udpClient.SendAsync(dummyData, dummyData.Length, _serverEndPoint);

                var localEndPoint = (IPEndPoint)_udpClient.Client.LocalEndPoint;
                l.log($"[VoiceClient] NAT-пакет отправлен с локального порта {localEndPoint.Port}.");
            }
            catch (Exception ex)
            {
                l.log($"[VoiceClient] Ошибка при отправке NAT-пакета: {ex.Message}");
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

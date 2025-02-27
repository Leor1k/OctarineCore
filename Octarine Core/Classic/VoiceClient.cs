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
        public int localendpoint;

        public VoiceClient()
        {
            try
            {
                _udpClient = new UdpClient(0); // Используем динамический порт
                _serverEndPoint = new IPEndPoint(IPAddress.Parse(_serverIp), _serverPort);
                _udpClient.Client.SendBufferSize = 65536; // 64 KB

                var localEndPoint = (IPEndPoint)_udpClient.Client.LocalEndPoint;
                localendpoint = localEndPoint.Port;
                l.log($"[VoiceClient] Клиент запущен на порту {localEndPoint.Port} и слушает с {_serverIp}:{_serverPort}");

                _waveIn = new WaveInEvent
                {
                    WaveFormat = new WaveFormat(16000, 16, 1) // 16kHz, 16 бит, моно (лучшее качество)
                };

                _waveIn.DataAvailable += OnAudioData;

                l.log("[VoiceClient] Обработчик DataAvailable привязан.");
            }
            catch (Exception ex)
            {
                l.log($"[VoiceClient] Ошибка инициализации: {ex.Message}");
            }
        }

        private async void OnAudioData(object sender, WaveInEventArgs e)
        {
            if (e.BytesRecorded > 0)
            {
                try
                {
                    await _udpClient.SendAsync(e.Buffer, e.BytesRecorded, _serverEndPoint);
                    l.log($"[VoiceClient] Отправлено {e.BytesRecorded} байт на {_serverEndPoint}");
                }
                catch (Exception ex)
                {
                    l.log($"[VoiceClient] Ошибка отправки аудиоданных: {ex.Message}");
                }
            }
        }

        public async Task PunchHole()
        {
            try
            {
                byte[] dummyData = Encoding.UTF8.GetBytes("NAT_HOLE");
                await _udpClient.SendAsync(dummyData, dummyData.Length, _serverEndPoint);
                l.log($"[VoiceClient] NAT-пакет отправлен.");
            }
            catch (Exception ex)
            {
                l.log($"[VoiceClient] Ошибка при отправке NAT-пакета: {ex.Message}");
            }
        }

        public void StartRecording()
        {
            try
            {
                _waveIn.StartRecording();
                l.log("[VoiceClient] Запись успешно начата.");
            }
            catch (Exception ex)
            {
                l.log($"[VoiceClient] Ошибка при запуске записи: {ex.Message}");
            }
        }

        public void StopRecording()
        {
            try
            {
                _waveIn.StopRecording();
                l.log("[VoiceClient] Запись остановлена.");
            }
            catch (Exception ex)
            {
                l.log($"[VoiceClient] Ошибка при остановке записи: {ex.Message}");
            }
        }
    }
}

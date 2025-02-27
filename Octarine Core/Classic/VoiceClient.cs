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
        private string _serverIp = "147.45.175.135"; 
        private int _serverPort = 5005; 
        private IPEndPoint _serverEndPoint;
        private WaveInEvent _waveIn;
        private Log l = new Log();
        public int localendpoint;
        private int BufferSize = 2048;

        public VoiceClient()
        {
            try
            {
                _udpClient = new UdpClient(0); 
                _serverEndPoint = new IPEndPoint(IPAddress.Parse(_serverIp), _serverPort);
                _udpClient.Client.SendBufferSize = 65536; 

                var localEndPoint = (IPEndPoint)_udpClient.Client.LocalEndPoint;
                localendpoint = localEndPoint.Port;
                l.log($"[VoiceClient] Клиент запущен на порту {localEndPoint.Port} и слушает с {_serverIp}:{_serverPort}");
                for (int i = 0; i < WaveIn.DeviceCount; i++)
                {
                    var capabilities = WaveIn.GetCapabilities(i);
                    l.log($"Микрофон {i}: {capabilities.ProductName}, Каналов: {capabilities.Channels}");
                }

                _waveIn = new WaveInEvent
                {
                    DeviceNumber = 0,
                    WaveFormat = new WaveFormat(16000, 16, 2),
                    BufferMilliseconds = 100
                };

                _waveIn.DataAvailable += OnAudioData;

                _waveIn.DataAvailable += (sender, e) =>
                {
                    l.log($"[VoiceClient] DataAvailable сработал! Получено {e.BytesRecorded} байт");
                };


            }
            catch (Exception ex)
            {
                l.log($"[VoiceClient] Ошибка инициализации: {ex.Message}");
            }
        }

        private async void OnAudioData(object sender, WaveInEventArgs e)
        {
            l.log($"[OnAudioData] байтов 0 {e.BytesRecorded} приелетело");
            if (e.BytesRecorded > 0)
            {
                try
                {
                    l.log($"[OnAudioData] попытка отправить{e.BytesRecorded} в {_serverEndPoint}");
                    int bytesToSend = Math.Min(e.BytesRecorded, BufferSize);
                    await _udpClient.SendAsync(e.Buffer, bytesToSend, _serverEndPoint);
                }
                catch (Exception ex)
                {
                    l.log($"Error sending audio: {ex.Message}");
                }
            }
        }
        public void StartRecording()
        {
            try
            {
                l.log("[StartRecording] Попытка начать запись...");
                _waveIn.StartRecording();
                l.log("[StartRecording] Запись успешно начата!");
            }
            catch (Exception ex)
            {
                l.log($"[StartRecording] Ошибка при запуске записи: {ex.Message}");
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

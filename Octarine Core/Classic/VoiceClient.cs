using System;
using System.Net;
using System.Net.Sockets;
using NAudio.Wave;

namespace Octarine_Core.Classic
{
    public class VoiceClient
    {
        public UdpClient _udpClient;
        private readonly string _serverIp = "147.45.175.135";
        private readonly int _serverPort = 5005;
        private IPEndPoint _serverEndPoint;
        private WaveInEvent _waveIn;
        private Log l = new Log();
        public int LocalPort { get; private set; }

        public VoiceClient()
        {
            try
            {
                _waveIn = new WaveInEvent
                {
                    DeviceNumber = 0,
                    WaveFormat = new WaveFormat(16000, 16, 2),
                    BufferMilliseconds = 100
                };
                _udpClient = new UdpClient(new IPEndPoint(IPAddress.Any, 0));
                LocalPort = ((IPEndPoint)_udpClient.Client.LocalEndPoint).Port;
                l.log1($"Создание UDP : {_udpClient.Client.LocalEndPoint}");
                _serverEndPoint = new IPEndPoint(IPAddress.Parse(_serverIp), _serverPort);
                _udpClient.Client.SendBufferSize = 65536;

                l.log1($"[VoiceClient] Клиент запущен на порту {LocalPort}, отправляет на {_serverIp}:{_serverPort}");

                

                _waveIn.DataAvailable += OnAudioData;
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
                    int roomId = Properties.Settings.Default.UserID; 

                    byte[] roomIdBytes = BitConverter.GetBytes(roomId);
                    byte[] packet = new byte[roomIdBytes.Length + e.BytesRecorded];

                    Buffer.BlockCopy(roomIdBytes, 0, packet, 0, roomIdBytes.Length);
                    Buffer.BlockCopy(e.Buffer, 0, packet, roomIdBytes.Length, e.BytesRecorded);

                    l.log($"[OnAudioData] Отправка {packet.Length} байт с Id {roomId} на {_serverEndPoint}");

                    await _udpClient.SendAsync(packet, packet.Length, _serverEndPoint);
                }
                catch (Exception ex)
                {
                    l.log($"[OnAudioData] Ошибка при отправке: {ex.Message}");
                }
            }
        }



        public void StartRecording()
        {
            try
            {
                l.log("[StartRecording] Запуск записи...");
                _waveIn.StartRecording();
                l.log("[StartRecording] Запись началась!");
            }
            catch (Exception ex)
            {
                l.log($"[StartRecording] Ошибка: {ex.Message}");
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
                l.log($"[VoiceClient] Ошибка при остановке: {ex.Message}");
            }
        }
    }
}

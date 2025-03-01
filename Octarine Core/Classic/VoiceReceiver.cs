using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using NAudio.Wave;

namespace Octarine_Core.Classic
{
    public class VoiceReceiver
    {
        private UdpClient _udpClient;
        private WaveOutEvent _waveOut;
        private BufferedWaveProvider _waveProvider;
        private Log l = new Log();
        private TaskCompletionSource<int> _portTaskSource = new TaskCompletionSource<int>(); 

        public VoiceReceiver()
        {
            _waveOut = new WaveOutEvent();
            _waveProvider = new BufferedWaveProvider(new WaveFormat(16000, 16, 2))
            {
                BufferDuration = TimeSpan.FromSeconds(10),
                DiscardOnBufferOverflow = true
            };
            _waveOut.Init(_waveProvider);
            _waveOut.Play();
        }

        public void SetUdpPort(int udpPort)
        {
            if (!_portTaskSource.Task.IsCompleted) 
            {
                _portTaskSource.SetResult(udpPort); 
            }
        }

        public async Task StartListening()
        {
            l.log1("[VoiceReceiver] Ожидание получения UDP-порта...");

            int port = await _portTaskSource.Task; 
            l.log1($"[VoiceReceiver] Получен реальный порт: {port}");
            Console.WriteLine($"[VoiceReceiver] Получен реальный порт: {port}");

            try
            {
                _udpClient = new UdpClient(new IPEndPoint(IPAddress.Any, port));
                _udpClient.Client.ReceiveBufferSize = 65536;
                l.log1($"[VoiceReceiver] Клиент слушает на порту {port}");

                while (true)
                {
                    UdpReceiveResult result = await _udpClient.ReceiveAsync();
                    byte[] receivedData = result.Buffer;
                    IPEndPoint sender = result.RemoteEndPoint;

                    if (receivedData.Length < 4)
                    {
                        l.log1($"[VoiceReceiver] Ошибка: слишком короткий пакет от {sender}");
                        continue;
                    }
                    int roomId = BitConverter.ToInt32(receivedData, 0);
                    byte[] audioData = new byte[receivedData.Length - 4];
                    Buffer.BlockCopy(receivedData, 4, audioData, 0, audioData.Length);
                    l.log1($"[VoiceReceiver] Получен аудиопакет для RoomID {roomId} от {sender}");
                    _waveProvider.AddSamples(audioData, 0, audioData.Length);
                }
            }
            catch (Exception ex)
            {
                l.log1($"[VoiceReceiver] Ошибка приёма данных: {ex.Message}");
            }
        }
    }
}

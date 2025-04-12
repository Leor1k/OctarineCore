using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using NAudio.Wave;

namespace Octarine_Core.Classic
{
    public class VoiceReceiver : IDisposable
    {
        private UdpClient _udpClient;
        private WaveOutEvent _waveOut;
        private BufferedWaveProvider _waveProvider;
        private Log l = new Log();
        private TaskCompletionSource<int> _portTaskSource = new TaskCompletionSource<int>();
        private bool _isDisposed;

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

        public async Task StartListening(UdpClient udpClient)
        {
            l.log1("[StartListening] Ожидание получения UDP-порта...");

            int port = await _portTaskSource.Task;
            
            Console.WriteLine($"[StartListening] Получен реальный UDP: {udpClient.Client.LocalEndPoint}");

            try
            {
                _udpClient = udpClient;
                _udpClient.Client.ReceiveBufferSize = 65536;;
                l.log1($"[StartListening] Клиент слушает на порту {_udpClient.Client.LocalEndPoint}----");

                while (true)
                {
                    l.log1($"[VoiceReceiver] Начало цикла");
                    UdpReceiveResult result = await _udpClient.ReceiveAsync();
                    byte[] receivedData = result.Buffer;
                    IPEndPoint sender = result.RemoteEndPoint;

                    if (receivedData.Length < 4)
                    {
                        l.log1($"[StartListening] Ошибка: слишком короткий пакет от {sender}");
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
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (_isDisposed) return;

            if (disposing)
            {
                // Освобождаем управляемые ресурсы
                _waveOut?.Stop();
                _waveOut?.Dispose();
                _udpClient?.Close();
                _udpClient?.Dispose();
            }

            _isDisposed = true;
        }

        ~VoiceReceiver()
        {
            Dispose(false);
        }
    }
}

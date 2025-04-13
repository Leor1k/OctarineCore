using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
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
        private bool _isListening;
        private CancellationTokenSource _cts;
        private Task _listeningTask;
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
        public void StartListening(UdpClient udpClient)
        {
            if (_isListening)
            {
                l.log1("[StartListening] Уже запущен");
                return;
            }

            l.log1("[StartListening] Запуск прослушивания...");
            _isListening = true;
            _cts = new CancellationTokenSource();
            _udpClient = udpClient;
            _udpClient.Client.ReceiveBufferSize = 65536;

            _listeningTask = Task.Run(() => ListenAsync(_cts.Token));
        }

        private async Task ListenAsync(CancellationToken token)
        {
            l.log1($"[ListenAsync] Клиент слушает на порту {_udpClient.Client.LocalEndPoint}");

            try
            {
                while (!token.IsCancellationRequested)
                {
                    l.log1($"[VoiceReceiver] Начало цикла");
                    UdpReceiveResult result;
                    try
                    {
                        result = await _udpClient.ReceiveAsync();
                    }
                    catch (ObjectDisposedException)
                    {
                        l.log1("[VoiceReceiver] Сокет был закрыт");
                        break;
                    }

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
            finally
            {
                _isListening = false;
                l.log1("[VoiceReceiver] Слушатель завершён.");
            }
        }

        public void StopListening()
        {
            if (!_isListening)
                return;

            l.log1("[VoiceReceiver] Остановка прослушивания...");
            _cts?.Cancel();
        }
    }
}

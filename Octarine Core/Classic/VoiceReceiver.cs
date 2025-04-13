using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
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
        private bool _isListening;
        private CancellationTokenSource _cts = new CancellationTokenSource();

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
            l.log1("[StartListening] Начало StartListening");
            if (_isListening)
            {
                l.log1("[StartListening] Уже запущен");
                return;
            }

            _isListening = true;
            _cts = new CancellationTokenSource();

            try
            {
                _udpClient = udpClient;
                _udpClient.Client.ReceiveBufferSize = 65536;
                l.log1($"[StartListening] Клиент слушает на порту {_udpClient.Client.LocalEndPoint}----");

                while (!_cts.Token.IsCancellationRequested)
                {
                    l.log1($"[VoiceReceiver] Начало цикла");
                    var result = await _udpClient.ReceiveAsync();

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
            catch (ObjectDisposedException)
            {
                l.log1("[VoiceReceiver] Поток остановлен — сокет закрыт");
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
            l.log1("[VoiceReceiver] Остановка прослушивания...");
            _cts.Cancel();
        }
    }
}
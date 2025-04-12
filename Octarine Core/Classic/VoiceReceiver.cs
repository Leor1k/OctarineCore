using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using NAudio.Wave;

namespace Octarine_Core.Classic
{
    public class VoiceReceiver : IDisposable
    {
        private readonly UdpClient _udpClient;
        private readonly WaveOutEvent _waveOut;
        private readonly BufferedWaveProvider _waveProvider;
        private readonly Log l = new Log();
        private readonly TaskCompletionSource<int> _portTaskSource = new TaskCompletionSource<int>();
        private bool _isDisposed;

        public VoiceReceiver(UdpClient udpClient)
        {
            _udpClient = udpClient;
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
                _portTaskSource.SetResult(udpPort);
        }

        public async Task StartListening()
        {
            await _portTaskSource.Task;
            l.log1($"[VoiceReceiver] Слушаем на {_udpClient.Client.LocalEndPoint}");

            try
            {
                while (true)
                {
                    var result = await _udpClient.ReceiveAsync();
                    var receivedData = result.Buffer;

                    if (receivedData.Length < 4)
                        continue;

                    int roomId = BitConverter.ToInt32(receivedData, 0);
                    byte[] audio = new byte[receivedData.Length - 4];
                    Buffer.BlockCopy(receivedData, 4, audio, 0, audio.Length);
                    _waveProvider.AddSamples(audio, 0, audio.Length);
                }
            }
            catch (Exception ex)
            {
                l.log1($"[VoiceReceiver] Ошибка: {ex.Message}");
            }
        }

        public void Dispose()
        {
            if (_isDisposed) return;

            _waveOut?.Stop();
            _waveOut?.Dispose();
            _isDisposed = true;
        }

        ~VoiceReceiver() => Dispose();
    }
}

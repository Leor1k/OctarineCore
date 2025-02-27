using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using NAudio.Wave;
using System.Collections.Generic;

namespace Octarine_Core.Classic
{
    public class VoiceReceiver
    {
        private UdpClient _udpClient;
        private int _port = 5005; // Порт для приёма данных
        private WaveOutEvent _waveOut;
        private BufferedWaveProvider _waveProvider;
        private Log l = new Log();
        private List<IPEndPoint> clients = new List<IPEndPoint>();

        public VoiceReceiver()
        {
            try
            {
                _udpClient = new UdpClient(new IPEndPoint(IPAddress.Any, _port));
                _udpClient.Client.ReceiveBufferSize = 65536; // 64 KB
                l.log($"[VoiceReceiver] Клиент слушает на порту {_port}");

                _waveOut = new WaveOutEvent();
                _waveProvider = new BufferedWaveProvider(new WaveFormat(16000, 16, 1)) // Улучшенное качество: 16kHz, 16 бит, моно
                {
                    BufferLength = 8192,
                    DiscardOnBufferOverflow = true
                };
                _waveOut.Init(_waveProvider);
                _waveOut.Play();
            }
            catch (Exception ex)
            {
                l.log($"[VoiceReceiver] Ошибка при инициализации: {ex.Message}");
            }
        }

        public async Task StartListening()
        {
            l.log("[VoiceReceiver] Ожидание голосовых данных...");

            while (true)
            {
                try
                {
                    UdpReceiveResult result = await _udpClient.ReceiveAsync();
                    byte[] receivedData = result.Buffer;
                    IPEndPoint sender = result.RemoteEndPoint;

                    if (!clients.Contains(sender))
                    {
                        clients.Add(sender);
                        l.log($"[VoiceReceiver] Добавлен новый источник: {sender}");
                    }

                    // Воспроизводим полученные данные
                    _waveProvider.AddSamples(receivedData, 0, receivedData.Length);
                    l.log($"[VoiceReceiver] Воспроизведено {receivedData.Length} байт от {sender}");
                }
                catch (Exception ex)
                {
                    l.log($"[VoiceReceiver] Ошибка приёма данных: {ex.Message}");
                }
            }
        }
    }
}

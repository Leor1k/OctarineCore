using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;
using System.Collections.Generic;

namespace Octarine_Core.Classic
{
    public class VoiceReceiver
    {
        private UdpClient _udpClient;
        private int _port = 5005;
        private string _serverIp = "147.45.175.135"; // Серверный IP
        private WaveOutEvent _waveOut;
        private BufferedWaveProvider _waveProvider;
        private Log l = new Log();
        private List<IPEndPoint> clients = new List<IPEndPoint>();

        public VoiceReceiver()
        {
            try
            {
                _udpClient = new UdpClient(new IPEndPoint(IPAddress.Any, _port));
                l.log($"[VoiceReceiver] Сервер слушает на 0.0.0.0:{_port}");
                _udpClient.Client.ReceiveBufferSize = 65536; // 64 KB

                _waveOut = new WaveOutEvent();
                _waveProvider = new BufferedWaveProvider(new WaveFormat(8000, 16, 1)) // 8kHz, 16 бит, моно
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
                        l.log($"[VoiceReceiver] Новый клиент: {sender}");
                    }

                    // Воспроизводим полученные данные
                    _waveProvider.AddSamples(receivedData, 0, receivedData.Length);
                    l.log($"[VoiceReceiver] Воспроизведено {receivedData.Length} байт от {sender}");

                    // Пересылаем голос другим клиентам
                    foreach (var client in clients)
                    {
                        if (!client.Equals(sender))
                        {
                            await _udpClient.SendAsync(receivedData, receivedData.Length, client);
                            l.log($"[VoiceReceiver] Переслано {receivedData.Length} байт на {client}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    l.log($"[VoiceReceiver] Ошибка приёма данных: {ex.Message}");
                }
            }
        }

        public async Task PunchHole()
        {
            try
            {
                byte[] dummyData = Encoding.UTF8.GetBytes("NAT_HOLE");
                await _udpClient.SendAsync(dummyData, dummyData.Length, new IPEndPoint(IPAddress.Parse(_serverIp), _port));
                l.log("[VoiceReceiver] Отправлен тестовый пакет.");
            }
            catch (Exception ex)
            {
                l.log($"[VoiceReceiver] Ошибка при отправке NAT-пакета: {ex.Message}");
            }
        }
    }
}

using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;

namespace Octarine_Core.Classic
{
    public class VoiceReceiver
    {
        private UdpClient _udpClient;
        private int _port = 5005;
        private string _serverIp = "147.45.175.135"; // Серверный IP
        private WaveOutEvent _waveOut;
        private BufferedWaveProvider _waveProvider;
        private Log l= new Log();


        public VoiceReceiver()
        {
            try
            {
                _udpClient = new UdpClient(new IPEndPoint(IPAddress.Any, _port));
                l.log($"[VoiceReceiver] Слушаем на 0.0.0.0:{_port}");
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
                l.log($"[VoiceReceiver] Ошибка при инициализации UdpClient: {ex.Message}");
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
                    string message = Encoding.UTF8.GetString(receivedData);
                    l.log($"[VoiceReceiver] Получено {result.Buffer.Length} байт от {result.RemoteEndPoint} (мб это: {message})");
                    l.log($"[VoiceReceiver] Пакет от {result.RemoteEndPoint} отклонён.");
                    _waveProvider.AddSamples(result.Buffer, 0, result.Buffer.Length);
                    l.log($"[VoiceReceiver] Добавлено в буфер {result.Buffer.Length} байт аудиоданных.");
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
                byte[] dummyData = new byte[1] { 0x00 };
                await _udpClient.SendAsync(dummyData, dummyData.Length, new IPEndPoint(IPAddress.Parse(_serverIp), _port));
                l.log("[VoiceReceiver] Отправлен тестовый пакет на сервер.");
            }
            catch (Exception ex)
            {
                l.log($"[VoiceReceiver] Ошибка при отправке NAT-пакета: {ex.Message}");
            }
        }
    }
}

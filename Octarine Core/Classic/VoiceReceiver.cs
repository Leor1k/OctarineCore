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
        private readonly string _logFilePath = "client_logs.txt"; // Файл для логов

        public VoiceReceiver()
        {
            try
            {
                _udpClient = new UdpClient(new IPEndPoint(IPAddress.Any, _port));
                Log($"[CLIENT] Слушаем на 0.0.0.0:{_port}");

                _waveOut = new WaveOutEvent();
                _waveProvider = new BufferedWaveProvider(new WaveFormat(8000, 16, 1))
                {
                    BufferLength = 8192,
                    DiscardOnBufferOverflow = true
                };
                _waveOut.Init(_waveProvider);
                _waveOut.Play();
            }
            catch (Exception ex)
            {
                Log($"[CLIENT] Ошибка при инициализации UdpClient: {ex.Message}");
            }
        }

        public async Task StartListening()
        {
            Log("[CLIENT] Ожидание голосовых данных...");
            while (true)
            {
                try
                {
                    UdpReceiveResult result = await _udpClient.ReceiveAsync();
                    byte[] receivedData = result.Buffer;
                    string message = Encoding.UTF8.GetString(receivedData);
                    Log($"[CLIENT] Получено {result.Buffer.Length} байт от {result.RemoteEndPoint} (мб это: {message})");

                    if (result.RemoteEndPoint.Address.ToString() != _serverIp)
                    {
                        Log($"[CLIENT] Пакет от {result.RemoteEndPoint} отклонён.");
                        continue;
                    }

                    _waveProvider.AddSamples(result.Buffer, 0, result.Buffer.Length);
                    Log($"[CLIENT] Добавлено в буфер {result.Buffer.Length} байт аудиоданных.");
                }
                catch (Exception ex)
                {
                    Log($"[CLIENT] Ошибка приёма данных: {ex.Message}");
                }
            }
        }

        public async Task PunchHole()
        {
            try
            {
                byte[] dummyData = new byte[1] { 0x00 };
                await _udpClient.SendAsync(dummyData, dummyData.Length, new IPEndPoint(IPAddress.Parse(_serverIp), _port));
                Log("[CLIENT] Отправлен тестовый пакет на сервер.");
            }
            catch (Exception ex)
            {
                Log($"[CLIENT] Ошибка при отправке NAT-пакета: {ex.Message}");
            }
        }

        private void Log(string message)
        {
            string logMessage = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}";
            Console.WriteLine(logMessage);

            try
            {
                File.AppendAllText(_logFilePath, logMessage + Environment.NewLine);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Ошибка записи в лог-файл: {ex.Message}");
            }
        }
    }
}

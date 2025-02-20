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
        private int _port = 5005;
        private string _ipAddress = "147.45.175.135"; // Привязываем к конкретному IP
        private WaveOutEvent _waveOut;
        private BufferedWaveProvider _waveProvider;

        public VoiceReceiver()
        {
            try
            {
                _udpClient = new UdpClient(new IPEndPoint(IPAddress.Parse(_ipAddress), _port));
                Console.WriteLine($"[CLIENT] Привязано к {_ipAddress}:{_port}");

                _waveOut = new WaveOutEvent();
                _waveProvider = new BufferedWaveProvider(new WaveFormat(8000, 16, 1))
                {
                    BufferLength = 8192, // Размер буфера
                    DiscardOnBufferOverflow = true
                };
                _waveOut.Init(_waveProvider);
                _waveOut.Play();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[CLIENT] Ошибка при инициализации: {ex.Message}");
            }
        }

        public async Task StartListening()
        {
            Console.WriteLine("[CLIENT] Ожидание голосовых данных...");
            while (true)
            {
                try
                {
                    UdpReceiveResult result = await _udpClient.ReceiveAsync();
                    Console.WriteLine($"[CLIENT] Получено {result.Buffer.Length} байт от {result.RemoteEndPoint}");

                    // Проверка источника
                    if (result.RemoteEndPoint.Address.ToString() != _ipAddress)
                    {
                        Console.WriteLine("[CLIENT] Игнорируем пакет от неизвестного отправителя.");
                        continue;
                    }

                    _waveProvider.AddSamples(result.Buffer, 0, result.Buffer.Length);
                    Console.WriteLine($"[CLIENT] Добавлено в буфер {result.Buffer.Length} байт аудиоданных.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[CLIENT] Ошибка приема данных: {ex.Message}");
                }
            }
        }

        public async Task PunchHole()
        {
            try
            {
                byte[] dummyData = new byte[1] { 0x00 };
                await _udpClient.SendAsync(dummyData, dummyData.Length, new IPEndPoint(IPAddress.Parse(_ipAddress), _port));
                Console.WriteLine("[CLIENT] Отправлен тестовый пакет для NAT.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[CLIENT] Ошибка при отправке NAT-пакета: {ex.Message}");
            }
        }
    }
}
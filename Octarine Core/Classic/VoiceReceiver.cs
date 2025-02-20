using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using NAudio.Wave;

namespace Octarine_Core.Classic
{
    public class VoiceReceiver
    {
        private UdpClient _udpClient;
        private int _port = 5005;
        private WaveOutEvent _waveOut;
        private BufferedWaveProvider _waveProvider;

        public VoiceReceiver()
        {
            _udpClient = new UdpClient(_port);
            _waveOut = new WaveOutEvent();
            _waveProvider = new BufferedWaveProvider(new WaveFormat(8000, 16, 1))
            {
                BufferLength = 8192, // Размер буфера
                DiscardOnBufferOverflow = true
            };
            _waveOut.Init(_waveProvider);
            _waveOut.Play();
        }

        public async Task StartListening()
        {
            while (true)
            {
                UdpReceiveResult result = await _udpClient.ReceiveAsync();
                Console.WriteLine($"[CLIENT] Получено {result.Buffer.Length} байт аудиоданных.");
                _waveProvider.AddSamples(result.Buffer, 0, result.Buffer.Length);
                Console.WriteLine($"[CLIENT] Добавлено в буфер {result.Buffer.Length} байт аудиоданных.");
            }
        }
    }
}

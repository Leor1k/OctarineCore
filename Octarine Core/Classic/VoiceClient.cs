using System;
using System.Net;
using System.Net.Sockets;
using NAudio.Wave;

namespace Octarine_Core.Classic
{
    public class VoiceClient : IDisposable
    {
        private readonly UdpClient _udpClient;
        private readonly IPEndPoint _serverEndPoint = new IPEndPoint(IPAddress.Parse("147.45.175.135"), 5005);
        private readonly WaveInEvent _waveIn;
        private readonly Log l = new Log();
        private bool _isDisposed;

        public VoiceClient(UdpClient udpClient)
        {
            _udpClient = udpClient;

            _waveIn = new WaveInEvent
            {
                DeviceNumber = 0, // не менять
                WaveFormat = new WaveFormat(16000, 16, 2),
                BufferMilliseconds = 100
            };

            _waveIn.DataAvailable += OnAudioData;

            l.log1($"[VoiceClient] Настроен UDP: {_udpClient.Client.LocalEndPoint}");
        }

        private async void OnAudioData(object sender, WaveInEventArgs e)
        {
            if (e.BytesRecorded <= 0) return;

            try
            {
                int roomId = Properties.Settings.Default.UserID;
                byte[] roomBytes = BitConverter.GetBytes(roomId);
                byte[] packet = new byte[roomBytes.Length + e.BytesRecorded];

                Buffer.BlockCopy(roomBytes, 0, packet, 0, roomBytes.Length);
                Buffer.BlockCopy(e.Buffer, 0, packet, roomBytes.Length, e.BytesRecorded);

                await _udpClient.SendAsync(packet, packet.Length, _serverEndPoint);
            }
            catch (Exception ex)
            {
                l.log($"[VoiceClient] Ошибка при отправке: {ex.Message}");
            }
        }

        public void StartRecording()
        {
            _waveIn.StartRecording();
            l.log("[VoiceClient] Запись началась");
        }

        public void StopRecording()
        {
            _waveIn.StopRecording();
            l.log("[VoiceClient] Запись остановлена");
        }

        public void Dispose()
        {
            if (_isDisposed) return;

            _waveIn.StopRecording();
            _waveIn.DataAvailable -= OnAudioData;
            _waveIn.Dispose();

            _isDisposed = true;
        }

        ~VoiceClient() => Dispose();
    }
}

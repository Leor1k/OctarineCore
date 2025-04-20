using System.IO;
using System;
using System.Windows;
using System.Threading.Tasks;

namespace Octarine_Core.Classic
{
    public class SoundController
    {
        private readonly SaundPlayer RingtonePlayer;
        private readonly SaundPlayer MessagePlayer;
        private readonly SaundPlayer AcceptPlayer;
        private readonly SaundPlayer DeniedPlayer;


        public SoundController()
        {
            string projectRoot = Path.GetDirectoryName(Path.GetDirectoryName(Directory.GetCurrentDirectory()));
            string soundDir = Path.Combine(projectRoot, "Resource", "Sounds");


            if (!Directory.Exists(soundDir))
            {
                MessageBox.Show($"Папка со звуками не найдена: {soundDir}");
                return;
            }

            string ringtonePath = Path.Combine(soundDir, "Melody.wav");
            string messagePath = Path.Combine(soundDir, "Sileens.wav");
            string AcceptPath = Path.Combine(soundDir, "Accept.wav");
            string DeniedPath = Path.Combine(soundDir, "Denied.wav");


            if (!File.Exists(ringtonePath))
            {
                MessageBox.Show($"Файл рингтона не найден: {ringtonePath}");
                return;
            }

            RingtonePlayer = new SaundPlayer(new Uri(ringtonePath).AbsoluteUri);
            MessagePlayer = new SaundPlayer(new Uri(messagePath).AbsoluteUri);
            AcceptPlayer = new SaundPlayer(new Uri(AcceptPath).AbsoluteUri);
            DeniedPlayer = new SaundPlayer(new Uri(DeniedPath).AbsoluteUri);

        }

        public void StartRingtone()
        {
            RingtonePlayer?.SetVolume(0.5);
            RingtonePlayer?.Play();
        }
        public void StopRingtone()
        {
            RingtonePlayer?.Dispose();
        }
        public async void StartAccept()
        {
            AcceptPlayer?.SetVolume(0.5);
            AcceptPlayer?.Play();

            await Task.Delay(530);

            AcceptPlayer?.Dispose();
        }
        public async void StartDenied()
        {
            DeniedPlayer?.SetVolume(0.5);
            DeniedPlayer?.Play();
            await Task.Delay(530);
            DeniedPlayer?.Dispose();
        }
        public async void StartMessage()
        {
            MessagePlayer?.SetVolume(0.5);
            MessagePlayer?.Play();
            await Task.Delay(530);
            MessagePlayer?.Dispose();
        }
    }
}
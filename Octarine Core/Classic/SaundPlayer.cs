using System;
using System.Windows.Media;

namespace Octarine_Core.Classic
{
    public class SaundPlayer
    {
        private readonly MediaPlayer _mediaPlayer;
        public SaundPlayer(string soundPath)
        {
            _mediaPlayer = new MediaPlayer();
            _mediaPlayer.Open(new Uri(soundPath));
            _mediaPlayer.Volume = 1.0;
            _mediaPlayer.MediaEnded += (sender, e) => _mediaPlayer.Position = TimeSpan.Zero;
        }
        public void Play()
        {
            if (_mediaPlayer != null)
            {
                _mediaPlayer.Position = TimeSpan.Zero;
                _mediaPlayer.Play();
            }
        }

        public void SetVolume(double volume)
        {
            if (_mediaPlayer != null)
            {
                
                _mediaPlayer.Volume = volume < 0 ? 0 : (volume > 1 ? 1 : volume);
            }
        }
        public void Dispose()
        {
            _mediaPlayer?.Close();
        }
    }
}
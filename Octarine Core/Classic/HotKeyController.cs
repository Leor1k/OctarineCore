using System.Windows;
using System.Windows.Input;
using GlobalHotKey;
using Octarine_Core.Autorisation;
using Octarine_Core.Resource.UsersIntefeces;

namespace Octarine_Core.Classic
{
    public class HotKeyController
    {
        private HotKeyManager _hotKeyManager;
        private OctarineWindow _octarine;
        private HotKey _muteMicHotKey;
        private HotKey _muteAllHotKey;


        public HotKeyController(OctarineWindow octarine)
        {
            _octarine = octarine;
            _hotKeyManager = new HotKeyManager();
            _hotKeyManager.KeyPressed += HotKeyManager_KeyPressed;

            LoadHotKeys();
        }

        private void LoadHotKeys()
        {
            var muteMicStr = Properties.Settings.Default.MuteMicroHotKey;
            if (!string.IsNullOrEmpty(muteMicStr))
            {
                _muteMicHotKey = HotKeyParser.Parse(muteMicStr);
                _hotKeyManager.Register(_muteMicHotKey);
                HotKeyBrick brik = new HotKeyBrick("Выключить/включить микрофон", "M", Properties.Settings.Default.MuteMicroHotKey,this);
                _octarine.HotKeyStack.Children.Add(brik);
            }
            var muteAllStr = Properties.Settings.Default.MuteAllVoiceHotKey;
            if (!string.IsNullOrEmpty(muteAllStr))
            {
                _muteAllHotKey = HotKeyParser.Parse(muteAllStr);
                _hotKeyManager.Register(_muteAllHotKey);
                HotKeyBrick brik = new HotKeyBrick("Выключить/включить звук в звонке", "N", Properties.Settings.Default.MuteAllVoiceHotKey, this);
                _octarine.HotKeyStack.Children.Add(brik);
            }
        }

        private void HotKeyManager_KeyPressed(object sender, KeyPressedEventArgs e)
        {
            _octarine.Dispatcher.Invoke(() =>
            {
                if (e.HotKey.Equals(_muteMicHotKey))
                {
                    _octarine.MuteMicro();
                }
                if (e.HotKey.Equals(_muteAllHotKey))
                {
                    _octarine.MuteVoice();
                }
            });
        }

        public void SetMuteMicHotKey(string hotkeyStr)
        {
            if (_muteMicHotKey != null)
            {
                _hotKeyManager.Unregister(_muteMicHotKey);
            }
            _muteMicHotKey = HotKeyParser.Parse(hotkeyStr);
            _hotKeyManager.Register(_muteMicHotKey);

            Properties.Settings.Default.MuteMicroHotKey = hotkeyStr;
            Properties.Settings.Default.Save();
        }
        public void SetMuteAllHotKey(string hotkeyStr)
        {
            if (_muteAllHotKey != null)
            {
                _hotKeyManager.Unregister(_muteAllHotKey);
            }
            _muteAllHotKey = HotKeyParser.Parse(hotkeyStr);
            try
            {
                _hotKeyManager.Register(_muteAllHotKey);
            }
            catch
            {

            }
            

            Properties.Settings.Default.MuteAllVoiceHotKey = hotkeyStr;
            Properties.Settings.Default.Save();
        }

        public void Dispose()
        {
            _hotKeyManager?.Dispose();
        }
    }
}
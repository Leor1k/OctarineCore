using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Octarine_Core.Classic;


namespace Octarine_Core.Resource.UsersIntefeces
{
    public partial class HotKeyBrick : UserControl
    {
        private static HotKeyController _controller;
        private bool ChangeHotKey = false;
        public HotKeyBrick(string textKey, string defaltKey, string setUpKey, HotKeyController hotKeyController)
        {
            InitializeComponent();
            TextKey.Text = textKey;
            DefaltKey.Content = defaltKey;
            SetUpKey.Content = setUpKey;
            _controller = hotKeyController;
        }

        private void SetUpKey_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ChangeHotKey = true;
            SetUpKey.Content = "...";
        }

        private void UserControl_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (ChangeHotKey == true)
            {
                if (e.Key == Key.LeftCtrl || e.Key == Key.RightCtrl || e.Key == Key.LeftAlt || e.Key == Key.RightAlt ||
                    e.Key == Key.LeftShift || e.Key == Key.RightShift || e.Key == Key.LWin || e.Key == Key.RWin || e.Key == Key.System)
                {

                }
                else
                {
                    if (DefaltKey.Content == "M")
                    {
                        MessageBox.Show(e.Key.ToString() + " Для микро");
                        if (e.Key.ToString() != Properties.Settings.Default.MuteAllVoiceHotKey)
                        {
                            ChangeHotKey = false;
                            SetUpKey.Content = e.Key.ToString();
                            e.Handled = true;
                            _controller.SetMuteMicHotKey(e.Key.ToString());
                        }

                    }
                    else if (DefaltKey.Content == "N")
                    {
                        MessageBox.Show(e.Key.ToString() + " Для Звука");
                        if (e.Key.ToString() != Properties.Settings.Default.MuteMicroHotKey)
                        {
                            ChangeHotKey = false;
                            SetUpKey.Content = e.Key.ToString();
                            e.Handled = true;
                            _controller.SetMuteAllHotKey(e.Key.ToString());
                        }
                    }
                }
            }
        }

        private void DefaltKey_Click(object sender, RoutedEventArgs e)
        {
            if (DefaltKey.Content == "M")
            {
                _controller.SetMuteMicHotKey(Key.M.ToString());
                SetUpKey.Content = "M";
            }
            else if (DefaltKey.Content == "N")
            {
                _controller.SetMuteAllHotKey(Key.M.ToString());
                SetUpKey.Content = "N";
            }
        }
    }
}

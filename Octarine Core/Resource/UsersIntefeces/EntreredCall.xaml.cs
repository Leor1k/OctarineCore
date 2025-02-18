using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Octarine_Core.Classic;

namespace Octarine_Core.Resource.UsersIntefeces
{
    /// <summary>
    /// Логика взаимодействия для EntreredCall.xaml
    /// </summary>
    public partial class EntreredCall : UserControl
    {
        public string RoomId { get; set; }
        public CallingController Controller { get; set; }
        private Timer _timer;

        public EntreredCall(string FriendName, string roomID, CallingController controller)
        {
            InitializeComponent();
            NameUser.Text = FriendName;
            Controller = controller;
            RoomId = roomID;

            _timer = new Timer(async _ =>
            {
                await Application.Current.Dispatcher.InvokeAsync(async () => await DeniedCall());
            }, null, 20000, Timeout.Infinite);
        }

        private async void AcceptCallBt_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //MessageBox.Show("Из метода");
                //MessageBox.Show(RoomId);
                await Controller.AcceptCallAsync(Properties.Settings.Default.UserID.ToString(), RoomId);
                //MessageBox.Show("Отправлено!");

                _timer?.Dispose();
                foreach (FriendBrick fb in Controller._octarine.ChatStack.Children)
                {
                    if (Convert.ToInt32(RoomId) == fb.ChatId)
                    {
                        await Controller._octarine.ShowUsersChat(fb.FriendName, fb.FriendIds[0]);
                        break;
                    }
                }
                Application.Current.Dispatcher.Invoke(() =>
                {
                    var parentContainer = VisualTreeHelper.GetParent(this) as Panel;
                    parentContainer?.Children.Remove(this);
                    foreach (ChatUpBur ch in Controller._octarine.IngoGrid.Children)
                    {
                        ch.StandartGrid.Visibility = Visibility.Hidden;
                        ch.CallGrid.Visibility = Visibility.Visible;
                    }
                });
                Properties.Settings.Default.InColling = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }

        private async void DeniedCallBtn_Click(object sender, RoutedEventArgs e)
        {
            await DeniedCall();
        }

        private async Task DeniedCall()
        {
            //MessageBox.Show("Биля телефона этого маму ебал");
            await Controller.RejectCallAsync(Properties.Settings.Default.UserID.ToString(), RoomId);
            Application.Current.Dispatcher.Invoke(() =>
            {
                var parentContainer = VisualTreeHelper.GetParent(this) as Panel;
                parentContainer?.Children.Remove(this);
            });

            _timer?.Dispose(); 
        }
    }
}

using System;
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
        public EntreredCall(string FriendName,string roomID, CallingController controller )
        {
            InitializeComponent();
            NameUser.Text = FriendName;
            Controller = controller;
            RoomId = roomID;
            
        }

        private async void AcceptCallBt_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                MessageBox.Show("Из метода");
                MessageBox.Show(RoomId.ToString());
                await Controller.AcceptCallAsync(Properties.Settings.Default.UserID.ToString(), RoomId);
                MessageBox.Show("Отправлено!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }

        }

        private async void DeniedCallBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Биля телефона этого маму ебал");
            await Controller.RejectCallAsync(Properties.Settings.Default.UserID.ToString(), RoomId);
            var parentContainer = VisualTreeHelper.GetParent(this) as Panel;

            if (parentContainer != null)
            {
                // Удаляем себя из контейнера
                parentContainer.Children.Remove(this);
            }
        }
    }
}

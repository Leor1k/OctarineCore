using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Octarine_Core.Classic;

namespace Octarine_Core.Resource.UsersIntefeces
{
    public partial class MoreUI : UserControl
    {
        private readonly ChatController controller;
        private readonly int ChatId;
        private readonly bool DeleteRoom;
        private readonly bool DeleteUser;
        private readonly SearchingFriend friend;

        public MoreUI(int chatId, bool deleteOrExit)
        {
            InitializeComponent();
            ChatId = chatId;
            if (DeleteRoom == false)
            {
                IfExit();
            }
            controller = Properties.Settings.Default.ChatController;
            DeleteRoom = deleteOrExit;

        }
        public MoreUI (SearchingFriend searchingFriend)
        {
            InitializeComponent();
            Zagolovok.Text = "Вы уверенны что хотите удалить пользователя из друзей?";
            basikText.Text = "Так же будет удалён приватный чат с этим пользователем.";
            DeleteUser = true;
            friend = searchingFriend;
        }
        private void IfExit()
        {
            Zagolovok.Text = "Вы уверенны что хотите покинуть этот чат?";
            basikText.Text = "Вы покинете этот чат и больше не будете иметь к нему доступ";
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if (DeleteUser == true)
            {
                friend.DeleteFriendShip();
            }
            else
            {
                if (DeleteRoom == true)
                {
                    await controller.DeleteChat(ChatId);
                }
                else
                {
                    int[] userId = { Properties.Settings.Default.UserID };
                    await controller.DeleteUserFromChat(ChatId, userId);
                }
            }
            CloseUI();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            CloseUI();
        }
        private void CloseUI()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var parentContainer = VisualTreeHelper.GetParent(this) as Panel;
                parentContainer?.Children.Remove(this);
            });
        }
    }
}

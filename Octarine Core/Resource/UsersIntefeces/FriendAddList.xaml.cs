using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using Octarine_Core.Classic;
using Octarine_Core.Models;

namespace Octarine_Core.Resource.UsersIntefeces
{

    public partial class FriendAddList : UserControl
    {
        AddFriendToRoomController controller;
        bool IsGroupChat;
        public FriendAddList(List<string> FriendsList, bool _isGroupChat)
        {
            InitializeComponent();
            IsGroupChat = _isGroupChat;
            controller = new AddFriendToRoomController();
            controller.LoadUsersFriend(MainStackPanel, FriendsList );
            if (IsGroupChat == false)
            {
                ChatNameTb.Visibility = Visibility.Visible;
                ChatNameTb.Text = $"Чат {Properties.Settings.Default.UserName}";
            }
            else
            {
                ChatNameTb.Visibility = Visibility.Hidden;
            }
        }

        private void CloseBTN_Click(object sender, RoutedEventArgs e)
        {
            CloseContol();
        }
        private void CloseContol ()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var parentContainer = VisualTreeHelper.GetParent(this) as Panel;
                parentContainer?.Children.Remove(this);
            });
            Properties.Settings.Default.InColling = false;
        }

        private void AddBTN_Click(object sender, RoutedEventArgs e)
        {
            if (!controller.ChangesUsersCaunt())
            {
                ErrorAutUIController er = new ErrorAutUIController();
                er.ShowUserError("Список добавленных друзей пуст", Properties.Settings.Default.BorderForEror);
            }
            else if (ChatNameTb.Text.Trim().Length ==0)
            {
                ErrorAutUIController er = new ErrorAutUIController();
                er.ShowUserError("У чата обязательно должно быть название", Properties.Settings.Default.BorderForEror);
            }
            else
            {
                CloseContol();
                GroupChat newchat= controller.ReturnChat();
                newchat.ChatName = ChatNameTb.Text;
                Properties.Settings.Default.ChatController.CreateGroupChat(newchat);
            }
        }
    }
}

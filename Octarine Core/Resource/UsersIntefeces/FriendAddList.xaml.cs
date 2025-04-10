using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Octarine_Core.Classic;
using Octarine_Core.Models;

namespace Octarine_Core.Resource.UsersIntefeces
{

    public partial class FriendAddList : UserControl
    {
        private AddFriendToRoomController controller;
        private bool IsGroupChat;
        private readonly int chatId;

        public FriendAddList(List<string> FriendsList)
        {
            InitializeComponent();
            IsGroupChat = false;
            controller = new AddFriendToRoomController();
            controller.LoadUsersFriend(MainStackPanel, FriendsList );
            ChatNameTb.Text = $"Чат {Properties.Settings.Default.UserName}";

        }
        public FriendAddList(List<string> FriendsList, string RoomName, int IdChat)
        {
            InitializeComponent();
            IsGroupChat = true;
            controller = new AddFriendToRoomController();
            controller.LoadUsersFriend(MainStackPanel, FriendsList);
            ChatNameTb.Text = RoomName;
            ChatNameTb.IsReadOnly = true;
            chatId = IdChat;
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
                er.ShowUserError("Список добавленных друзей пуст", Properties.Settings.Default.BorderForEror, false);
            }
            else if (ChatNameTb.Text.Trim().Length ==0)
            {
                ErrorAutUIController er = new ErrorAutUIController();
                er.ShowUserError("У чата обязательно должно быть название", Properties.Settings.Default.BorderForEror, false);
            }
            else
            {
                if (IsGroupChat == false)
                {
                    CreateGroupChat();
                }
                else
                {
                    AddserToToom();
                }
            } 
        }
        private void AddserToToom ()
        {
            CloseContol();
            var UsersList = controller.ReturnNewParticants();
            UsersList.ChatId = chatId;
            Properties.Settings.Default.ChatController.AddUsersInChat(UsersList);
        }
        private void CreateGroupChat ()
        {
            CloseContol();
            GroupChat newchat = controller.ReturnChat();
            newchat.ChatName = ChatNameTb.Text;
            Properties.Settings.Default.ChatController.CreateGroupChat(newchat);
        }
    }
}

using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using Octarine_Core.Classic;
using Octarine_Core.Models;

namespace Octarine_Core.Resource.UsersIntefeces
{
    /// <summary>
    /// Логика взаимодействия для ChatUpBur.xaml
    /// </summary>
    public partial class ChatUpBur : UserControl
    {
        string FriendName { get; set; } =string.Empty;
        int FriendId { get; set; } = 0;
        CallingController Controller { get; set; }
        List<string> FriendsList { get; set; }
        public string ChatId { get; set; }
        public ChatUpBur(string friendName, int friendId, CallingController controller, FriendBrick friend)
        {
            InitializeComponent();
            FriendName = friendName;
            FriendId = friendId;
            FriendNameTextBox.Text = friendName;
            Controller = controller;
            List<string> friendsList = new List<string>();
            foreach (var Users in friend.FriendIds)
            {
                friendsList.Add(Users.ToString());
            }
            FriendsList = friendsList;
            ChatId = friend.ChatId.ToString();
        }

        private async void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {

            //MessageBox.Show($"Друзья {string.Join(", ", FriendsList)}");
            StandartGrid.Visibility = System.Windows.Visibility.Hidden;
            CallGrid.Visibility = System.Windows.Visibility.Visible;
            t1.Text = Properties.Settings.Default.UserName;
            t2.Text = FriendName;
            CallRequest req = new CallRequest(ChatId, Properties.Settings.Default.UserID.ToString(), FriendsList);
            await Controller.StartCallAsync(req);
            Properties.Settings.Default.InColling = true;

        }

        private void EndCallBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            StandartGrid.Visibility = System.Windows.Visibility.Visible;
            CallGrid.Visibility = System.Windows.Visibility.Hidden;
            Properties.Settings.Default.InColling = false;
        }
    }
}

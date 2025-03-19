using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Octarine_Core.Classic;
using Octarine_Core.Models;

namespace Octarine_Core.Resource.UsersIntefeces
{
    /// <summary>
    /// Логика взаимодействия для ChatUpBur.xaml
    /// </summary>
    public partial class ChatUpBur : UserControl
    {
        string FriendName { get; set; } = string.Empty;
        int FriendId { get; set; } = 0;
        CallingController Controller { get; set; }
        List<string> FriendsList { get; set; }
        public string ChatId { get; set; }

        public DispatcherTimer _callTimer;

        public ChatUpBur(string friendName, int friendId, CallingController controller, FriendBrick friend)
        {
            InitializeComponent();
            FriendName = friendName;
            FriendId = friendId;
            FriendNameTextBox.Text = friendName;
            Controller = controller;
            FriendsList = new List<string>();


            foreach (var Users in friend.FriendIds)
            {
                FriendsList.Add(Users.ToString());
            }

            ChatId = friend.ChatId.ToString();

            _callTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(20)
            };
            _callTimer.Tick += (s, e) => EndCall();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            StandartGrid.Visibility = Visibility.Hidden;
            CallGrid.Visibility = Visibility.Visible;
            t1.Text = Properties.Settings.Default.UserName;
            t2.Text = FriendName;

            CallRequest req = new CallRequest(ChatId, Properties.Settings.Default.UserID.ToString(), FriendsList);
            await Controller.StartCallAsync(req);
            Properties.Settings.Default.InColling = true;

            _callTimer.Start();
            
        }

        private async void EndCallBtn_Click(object sender, RoutedEventArgs e)
        {
            await EndCall();
        }

        public async Task EndCall()
        {
            _callTimer.Stop();
            await Controller.EndCall(Properties.Settings.Default.UserID.ToString(), ChatId);
            StandartGrid.Visibility = Visibility.Visible;
            CallGrid.Visibility = Visibility.Hidden;
            Properties.Settings.Default.InColling = false;
        }
    }
}

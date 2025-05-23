﻿using System;
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
        SoundController sound;
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
            sound = new SoundController();
            sound.StartRingtone();
        }

        private async void AcceptCallBt_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _timer?.Dispose();
                foreach (FriendBrick fb in Controller._octarine.ChatStack.Children)
                {
                    if (Convert.ToInt32(RoomId) == fb.ChatId)
                    {
                        await Controller._octarine.ShowUsersChat(fb.FriendName, fb.FriendIds[0], fb);
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
                await Controller.AcceptCallAsync(Properties.Settings.Default.UserID.ToString(), RoomId);
            }
            catch (Exception ex)
            {
                var eror = new ErrorAutUIController();
                eror.ShowUserError($"Ошибка: {ex.Message}", Properties.Settings.Default.BorderForEror, false);
            }
            sound.StopRingtone();
        }

        private async void DeniedCallBtn_Click(object sender, RoutedEventArgs e)
        {
            await DeniedCall();
        }

        private async Task DeniedCall()
        {
            await Controller.RejectCallAsync(Properties.Settings.Default.UserID.ToString(), RoomId);
            Application.Current.Dispatcher.Invoke(() =>
            {
                var parentContainer = VisualTreeHelper.GetParent(this) as Panel;
                parentContainer?.Children.Remove(this);
            });
            _timer?.Dispose();
            Properties.Settings.Default.InColling = false;
            sound.StopRingtone();
        }
    }
}

using System;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Microsoft.AspNetCore.SignalR.Client;
using Octarine_Core.Models;
using Octarine_Core.Resource.UsersIntefeces;

namespace Octarine_Core.Classic
{
    internal class ChatHub
    {
        private HubConnection _connection;
        private StackPanel _stackPanel;
        public StackPanel _stackPanelForNewRequestFriends;
        private ChatController _chatController;

        public ChatHub(StackPanel stackPanel, ChatController chatController)
        {
            _stackPanel = stackPanel;
            _chatController = chatController;
            _connection = new HubConnectionBuilder()
     .WithUrl($"http://147.45.175.135:5000/chatHub?userId={Properties.Settings.Default.UserID}")
     .Build();

            _connection.On<MessagesDTO>("ReceiveMessage", (message) =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    if (message.ChatId == Properties.Settings.Default.IdActiveChat)
                    {

                        ///ToDo тут надо в api signal r чуть подефать
                        MessageBrick mb = new MessageBrick(message.Content, "Не забудь тут поправть", DateTime.Now.ToString("H:mm dd/mm/yyyy"), message.SenderId);
                        mb.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                        _stackPanel.Children.Add(mb);
                    }
                    else
                    {
                        SoundController controller = new SoundController();
                        controller.StartMessage();
                    }
                    
                });
            });
            _connection.On<FriendSignalR>("NewRequestOnFriend", (ForSendFriendRequest) =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    WhantsBeFriend whantsBeFriend = new WhantsBeFriend();
                    whantsBeFriend.FriendId = ForSendFriendRequest.FriendId;
                    whantsBeFriend.UserId = ForSendFriendRequest.UserId;
                    whantsBeFriend.UserName = ForSendFriendRequest.UserName;
                    whantsBeFriend.PhotoName = ForSendFriendRequest.PhotoName;
                    _stackPanelForNewRequestFriends.Children.Add(whantsBeFriend.CreateAcceptBrick(_chatController));

                });
            });
        }

        public async Task StartConnectionAsync()
        {
            try
            {
                await _connection.StartAsync();
            }
            catch (Exception ex)
            {
                var eror = new ErrorAutUIController();
                eror.ShowUserError($"Ошибка подключения к SignalR: {ex.Message}", Properties.Settings.Default.BorderForEror, false);
            }
        }
        public async Task<long> PingApi()
        {
            var __connection = new HubConnectionBuilder()
     .WithUrl($"http://147.45.175.135:5000/chatHub?userId=0")
     .Build();
            var stopwatch = Stopwatch.StartNew();
            try
            {
                await __connection.StartAsync();
                await __connection.StopAsync();
                return stopwatch.ElapsedMilliseconds;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        public async Task SendMessageAsync(string userId, MessagesDTO message)
        {
            try
            {
                await _connection.InvokeAsync("SendMessageToUser", userId, message);
            }
            catch (Exception ex)
            {
                var eror = new ErrorAutUIController();
                eror.ShowUserError("Не удалость отправить сообщение", Properties.Settings.Default.BorderForEror, false);
            }
        }
    }
}
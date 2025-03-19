using System;
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

            // Подписываемся на метод "ReceiveMessage"
            _connection.On<MessagesDTO>("ReceiveMessage", (message) =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    if (message.ChatId == Properties.Settings.Default.IdActiveChat)
                    {
                        MessageBrick mb = new MessageBrick(message.Content, true, DateTime.Now.ToString("H:mm dd/mm/yyyy"), message.SenderId);
                        mb.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                        _stackPanel.Children.Add(mb);
                    }
                    else
                    {
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
                MessageBox.Show($"Ошибка подключения к SignalR: {ex.Message}");
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
                MessageBox.Show($"Ошибка отправки сообщения: {ex.Message}");
            }
        }
    }
}
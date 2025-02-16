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

        public ChatHub(StackPanel stackPanel)
        {
            _stackPanel = stackPanel;
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
                        MessageBrick mb = new MessageBrick(message.Content, true);
                        mb.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                        _stackPanel.Children.Add(mb);
                    }
                    else
                    {
                    }
                    
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

        // Метод для отправки сообщения через SignalR
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
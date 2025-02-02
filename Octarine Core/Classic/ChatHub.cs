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
            MessageBox.Show($"UserID для подключения: {Properties.Settings.Default.UserID}");
            _stackPanel = stackPanel;
            _connection = new HubConnectionBuilder()
     .WithUrl($"http://147.45.175.135:5000/chatHub?userId={Properties.Settings.Default.UserID}")
     .Build();

            // Подписываемся на метод "ReceiveMessage"
            _connection.On<MessagesDTO>("ReceiveMessage", (message) =>
            {
                MessageBox.Show($"Получено сообщение: {message.Content}");
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
                        MessageBox.Show($"В другой диалог прилетело{message.Content}");
                    }
                    
                });
            });
        }

        public async Task StartConnectionAsync()
        {
            try
            {
                await _connection.StartAsync();
                MessageBox.Show($"Подключение к SignalR установлено.{Properties.Settings.Default.UserID}");
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
                MessageBox.Show("Сообщение отправлено через SignalR.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка отправки сообщения: {ex.Message}");
            }
        }
    }
}
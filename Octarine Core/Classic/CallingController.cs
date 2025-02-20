
using Microsoft.AspNetCore.SignalR.Client;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Windows;
using Octarine_Core.Resource.UsersIntefeces;
using Octarine_Core.Autorisation;

namespace Octarine_Core.Classic
{
    public class CallingController
    {
        private HubConnection _connection;

        public OctarineWindow _octarine;
        private VoiceReceiver _voiceReceiver;
        private VoiceClient _voiceClient;
        public CallingController(OctarineWindow octarineWindow)
        {
            _voiceReceiver = new VoiceReceiver();
            _voiceClient = new VoiceClient();
            _octarine = octarineWindow;
            _connection = new HubConnectionBuilder()
                .WithUrl($"http://147.45.175.135:5000/voiceHub?userId={Properties.Settings.Default.UserID}")
                .Build();
            _connection.On<string, string>("IncomingCall", (roomId, callerId) =>
            {
                _octarine.Dispatcher.Invoke(() =>
                {
                    string FriendName = "NIIK";
                    foreach (FriendBrick friend in _octarine.ChatStack.Children)
                    {
                        if (Convert.ToInt32(roomId) == friend.ChatId)
                        {
                            FriendName = friend.FriendName;
                            break;
                        }
                    }
                    //MessageBox.Show(roomId);
                    EntreredCall ec = new EntreredCall(FriendName, roomId, this);
                    _octarine.MainGrid.Children.Add(ec);
                });
            });


            _connection.On<string, List<string>>("CallAccepted", (roomId, participants) =>
            {
                //MessageBox.Show($"Звонок принят в комнате {roomId}, участники: {string.Join(", ", participants)}");
                // Обработать начало звонка: например, подключение к аудио-серверу
            });
            _connection.On<string>("Error", message =>
            {
                //MessageBox.Show($"Ошибка: {message}");
            });
        }
        public async Task StartConnectionAsync()
        {
            try
            {
                await _connection.StartAsync();
                MessageBox.Show($"Подключение к Voice установлено.{Properties.Settings.Default.UserID}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка подключения к Voice: {ex.Message}");
            }
        }

        public async Task StartCallAsync(string roomID, string callerId, List<string> participantIds)
        {
            //MessageBox.Show($"Полетела в StartCall с : ID{roomID}, {callerId} и { participantIds}");
            await _connection.SendAsync("StartCall", roomID, callerId, participantIds);
            _voiceClient.StartRecording();
            await Task.Run(() => _voiceReceiver.StartListening()); 
        }

        public async Task AcceptCallAsync(string userId, string roomId)
        {
            //MessageBox.Show("В методе");
            await _connection.SendAsync("AcceptCall", userId, roomId);
            _voiceClient.StartRecording();
            await Task.Run(() => _voiceReceiver.StartListening()); 
        }

        public async Task RejectCallAsync(string userId, string roomId)
        {
            await _connection.SendAsync("RejectCall", userId, roomId);
            _voiceClient.StopRecording(); 
        }
    }
}

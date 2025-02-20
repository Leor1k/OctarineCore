using Microsoft.AspNetCore.SignalR.Client;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Windows;
using Octarine_Core.Resource.UsersIntefeces;
using Octarine_Core.Autorisation;
using System.Net.Sockets;
using System.Text;

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
            _octarine  = octarineWindow;

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
                    Console.WriteLine($"[CALL] Входящий звонок от {callerId} в комнате {roomId}");
                    EntreredCall ec = new EntreredCall(FriendName, roomId, this);
                    _octarine.MainGrid.Children.Add(ec);
                });
            });

            _connection.On<string, List<string>>("CallAccepted", (roomId, participants) =>
            {
                Console.WriteLine($"[CALL] Звонок принят. Комната: {roomId}, участники: {string.Join(", ", participants)}");
            });

            _connection.On<string>("Error", message =>
            {
                Console.WriteLine($"[ERROR] {message}");
            });
        }

        public async Task StartConnectionAsync()
        {
            try
            {
                await _connection.StartAsync();
                Console.WriteLine($"[SIGNALR] Подключение установлено. UserID: {Properties.Settings.Default.UserID}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Ошибка подключения: {ex.Message}");
            }
        }

        public async Task StartCallAsync(string roomID, string callerId, List<string> participantIds)
        {
            Console.WriteLine($"[CALL] Начало вызова. Комната: {roomID}, Caller: {callerId}, Участники: {string.Join(", ", participantIds)}");
            await _connection.SendAsync("StartCall", roomID, callerId, participantIds);

            await _voiceReceiver.StartListening();
            _voiceClient.StartRecording();
        }

        public async Task AcceptCallAsync(string userId, string roomId)
        {
            Console.WriteLine($"[CALL] Принят вызов. User: {userId}, Комната: {roomId}");
            await _connection.SendAsync("AcceptCall", userId, roomId);

            await _voiceReceiver.StartListening();
            _voiceClient.StartRecording();
        }

        public async Task RejectCallAsync(string userId, string roomId)
        {
            Console.WriteLine($"[CALL] Отклонён вызов. User: {userId}, Комната: {roomId}");
            await _connection.SendAsync("RejectCall", userId, roomId);
            _voiceClient.StopRecording();
        }
    }
}

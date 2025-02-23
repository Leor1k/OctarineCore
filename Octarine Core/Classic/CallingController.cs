using Microsoft.AspNetCore.SignalR.Client;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Windows;
using Octarine_Core.Resource.UsersIntefeces;
using Octarine_Core.Autorisation;
using System.Net.Sockets;
using System.Text;
using Octarine_Core.Apis;
using Octarine_Core.Models;

namespace Octarine_Core.Classic
{
    public class CallingController
    {
        private HubConnection _connection;
        public OctarineWindow _octarine;
        private VoiceReceiver _voiceReceiver;
        private VoiceClient _voiceClient;
        private Log l = new Log();
        ApiRequests apir;

        public CallingController(OctarineWindow octarineWindow)
        {
            _voiceReceiver = new VoiceReceiver();
            _voiceClient = new VoiceClient();
            _octarine = octarineWindow;
            apir = new ApiRequests();

            _connection = new HubConnectionBuilder()
                .WithUrl($"http://147.45.175.135:5001/voiceHub?userId={Properties.Settings.Default.UserID}")
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
                    l.log($"[CallingController] Входящий звонок от {callerId} в комнате {roomId}");
                    EntreredCall ec = new EntreredCall(FriendName, roomId, this);
                    _octarine.MainGrid.Children.Add(ec);
                });
            });

            _connection.On<string, List<string>>("CallAccepted", (roomId, participants) =>
            {
                l.log($"[CallingController] Звонок принят. Комната: {roomId}, участники: {string.Join(", ", participants)}");
            });

            _connection.On<string>("Error", message =>
            {
                l.log($"[ERROR] {message}");
            });
        }

        public async Task StartConnectionAsync()
        {
            try
            {
                await _connection.StartAsync();
                l.log($"[CallingController] Подключение установлено. UserID: {Properties.Settings.Default.UserID}");
            }
            catch (Exception ex)
            {
                l.log($"[CallingController] Ошибка подключения: {ex.Message}");
            }
        }

        public async Task StartCallAsync(CallRequest request)
        {
            l.log($"[StartCallAsync] Начало вызова. Комната: {request.RoomId}, Caller: {request.CallerId}, Участники: {string.Join(", ", request.ParticipantIds)}");
            try
            {
                var message = await apir.PostAsync<object>(Properties.Settings.Default.StartCallAPI, request);
                l.log($"[StartCallAsync] Отправил в start-call");
            }
            catch (Exception ex)
            {
                l.log($"[StartCallAsync] Возникла ошибка: {ex.Message}");
            }

            await _voiceReceiver.StartListening();
            _voiceClient.StartRecording();
        }

        public async Task AcceptCallAsync(string userId, string roomId)
        {
            l.log($"[CallingController] Принят вызов. User: {userId}, Комната: {roomId}");
            var CallConfirmation = new 
            {
                RoomId = roomId,
                UserId = userId
            }; 
            try
            {
                var message = await apir.PostAsync<object>(Properties.Settings.Default.ConfirmCallAPI, CallConfirmation);
                l.log($"[StartCallAsync] Отправил в confirm-call");
            }
            catch (Exception ex)
            {
                l.log($"[StartCallAsync] Возникла ошибка: {ex.Message}");
            }
            await _voiceReceiver.StartListening();
            _voiceClient.StartRecording();
        }

        public async Task RejectCallAsync(string userId, string roomId)
        {
            l.log($"[CallingController] Отклонён вызов. User: {userId}, Комната: {roomId}");
            await _connection.SendAsync("RejectCall", userId, roomId);
            _voiceClient.StopRecording();
        }
    }
}

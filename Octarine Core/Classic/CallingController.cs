using Microsoft.AspNetCore.SignalR.Client;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Octarine_Core.Resource.UsersIntefeces;
using Octarine_Core.Autorisation;
using Octarine_Core.Apis;
using Octarine_Core.Models;
using NAudio.Wave;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Net.Sockets;
using System.Net;

namespace Octarine_Core.Classic
{
    public class CallingController
    {
        private HubConnection _connection;
        public OctarineWindow _octarine;
        private VoiceReceiver _voiceReceiver;
        public VoiceClient _voiceClient;
        private Log l = new Log();
        ApiRequests apir;
        public UdpClient udpClient;
        

        public CallingController(OctarineWindow octarineWindow)
        {
            for (int i = 0; i < WaveIn.DeviceCount; i++)
            {
                var capabilities = WaveIn.GetCapabilities(i);
                l.log($"Микрофон {i}: {capabilities.ProductName}");
            }
            CreateNewUdpPort();
            _voiceReceiver = new VoiceReceiver(udpClient);
            _voiceClient = new VoiceClient(udpClient);
            _octarine = octarineWindow;
            apir = new ApiRequests();
            Properties.Settings.Default.UserPort = 0;

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

            _connection.On<string, string>("UserJoinedCall", (RoomId, UserId) =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    l.log($"[UserJoinedCall] User {UserId} подключился в комнату с {RoomId}");
                    foreach (var item in octarineWindow.IngoGrid.Children)
                    {
                        if (item.GetType() == typeof(ChatUpBur))
                        {
                            ChatUpBur cub = item as ChatUpBur;
                            cub._callTimer.Stop();
                        }
                    }
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
            _connection.On<string, string>("RejectEndCall", (RoomId, UserId) =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    foreach (var item in octarineWindow.MainGrid.Children)
                    {
                        if (item.GetType() == typeof(EntreredCall))
                        {
                            EntreredCall ec = item as EntreredCall;
                            var parentContainer = VisualTreeHelper.GetParent(ec) as Panel;
                            parentContainer?.Children.Remove(ec);
                        }
                    }
                });
            });
            _connection.On<string, string>("UserLeftCall", (RoomId, UserId) =>
            {
                Application.Current.Dispatcher.Invoke(async () =>
                {
                    foreach (var item in octarineWindow.MainGrid.Children)
                    {
                        if (item.GetType() == typeof(ChatUpBur))
                        {
                            ChatUpBur cub = item as ChatUpBur;
                            await cub.EndCall();
                        }
                    }
                });
            });


        }
        private void StopVoiceAndreciver()
        {
            l.log1("[StopVoiceAndreciver] Начал очистку");
            l.log1($"[StopVoiceAndreciver] был порт {udpClient.Client.LocalEndPoint}");
            udpClient.Close();
            l.log1("[StopVoiceAndreciver] закрыл клиент");
            _voiceClient.StopRecording();
            _voiceReceiver.StopListening();
            CreateNewUdpPort();
            l.log1($"[StopVoiceAndreciver] Пересоздал порт {udpClient.Client.LocalEndPoint}");

        }
        public void CreateNewUdpPort()
        {
            udpClient = new UdpClient(new IPEndPoint(IPAddress.Any, 0));
        }
        public async Task StartConnectionAsync()
        {
            try
            {
                await _connection.StartAsync();
                l.log($"[StartConnectionAsync] Подключение установлено. UserID: {Properties.Settings.Default.UserID}");
            }
            catch (Exception ex)
            {
                l.Ex($"[StartConnectionAsync] Ошибка:{ex.Message} : {ex.Source}");
            }
        }

        public async Task StartCallAsync(CallRequest request)
        {
            l.log1("=============================Начало звонка=============================");
            l.log($"[StartCallAsync] Начало вызова. Комната: {request.RoomId}, Caller: {request.CallerId}, Участники: {string.Join(", ", request.ParticipantIds)}");

            try
            {
                var message = await apir.PostAsync<object>(Properties.Settings.Default.StartCallAPI, request);
            }
            catch (Exception ex)
            {
                l.Ex($"[StartCallAsync] Ошибка:{ex.Message} : {ex.Source}");
            }
            _voiceReceiver.StartListening();

            l.log("[VoiceClient] Запуск записи...");
            _voiceClient.StartRecording();
            l.log("[VoiceClient] Запись запущена.");
        }





        public async Task AcceptCallAsync(string userId, string roomId)
        {
            l.log1("=============================Начаор звонка=============================");
            l.log($"[CallingController] Принят вызов. User: {userId}, Комната: {roomId}");
            var CallConfirmation = new
            {
                RoomId = roomId,
                UserId = userId,
            };
            try
            {
                l.log($"[CallingController] Отпрален запрос с  RoomId: {roomId}, UserId: {userId} и localendpoint {_voiceClient.LocalPort} ");
                var message = await apir.PostAsync<object>(Properties.Settings.Default.ConfirmCallAPI, CallConfirmation);
                l.log($"[StartCallAsync] Отправил в confirm-call");
            }
            catch (Exception ex)
            {
                l.Ex($"[StartCallAsync] Ошибка:{ex.Message} : {ex.Source}");
            }
            _voiceReceiver.StartListening();
            l.log("[VoiceClient] Вызов StartRecording()...");
            _voiceClient.StartRecording();
            l.log("[VoiceClient] Вызвался успешно StartRecording()...");
        }


        public async Task RejectCallAsync(string userId, string roomId)
        {
            l.log($"[RejectCallAsync] Отклонён вызов. User: {userId}, Комната: {roomId}");
            try
            {
                l.log($"[RejectCallAsync] Отпрален запрос с  RoomId: {roomId}, UserId: {userId}");
                var CallEndRequest = new
                {
                    UserId = userId,
                    RoomId = roomId,
                };
                await apir.PostAsync<object>(Properties.Settings.Default.RejectCall, CallEndRequest);
            }
            catch (Exception ex)
            {
                l.Ex($"[RejectCallAsync] Ошибка:{ex.Message} : {ex.Source}");
            }
        }
        public async Task EndCall(string userId, string roomId)
        {
            l.log1("=============================Конец звонка=============================");
            l.log($"[EndCall] Отмена. User: {userId}, Комната: {roomId}");
            var CallEndRequest = new
            {
                RoomId = roomId,
                UserId = userId,
            };
            try
            {
                var message = await apir.PostAsync<object>(Properties.Settings.Default.EndCall, CallEndRequest);
                l.log($"[EndCall] Отправил в end-call");
            }
            catch (Exception ex)
            {
                l.Ex($"[EndCall] Ошибка:{ex.Message} : {ex.Source}");
            }
            StopVoiceAndreciver();
        }

    }
}
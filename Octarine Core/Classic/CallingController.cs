using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.AspNetCore.SignalR.Client;
using NAudio.Wave;
using Octarine_Core.Apis;
using Octarine_Core.Autorisation;
using Octarine_Core.Models;
using Octarine_Core.Resource.UsersIntefeces;

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
        private ChatUpBur optimalChat;
        private readonly SoundController soundController;

        public CallingController(OctarineWindow octarineWindow)
        {
            for (int i = 0; i < WaveIn.DeviceCount; i++)
            {
                var capabilities = WaveIn.GetCapabilities(i);
                l.log($"Микрофон {i}: {capabilities.ProductName}");
            }
            CreateNewUdpPort();
            soundController = new SoundController();
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
                    optimalChat._callTimer.Stop();
                    optimalChat.LoadUserDataInCall(Convert.ToInt32(UserId));
                    soundController.StartAccept();
                    ///Todo тест1
                });
            });

            _connection.On<List<string>>("UsersDataForCall", (listID) =>
            {
                ///Todo пусто
                foreach (var item in listID)
                {
                    optimalChat.LoadUserDataInCall(Convert.ToInt32(item));
                    MessageBox.Show($"Info {item}");
                }
            });

            _connection.On<string>("Error", message =>
            {
                ///Todo пусто2
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
                            soundController.StartDenied();
                            break;
                        }
                    }
                });
            });
            _connection.On<string, string>("UserLeftCall", (RoomId, UserId) =>
            {
                ///Todo тест2
                Application.Current.Dispatcher.Invoke(() =>
                {
                    optimalChat.RemoveUserFromRoom(Convert.ToInt32(UserId));
                    soundController.StartDenied();
                });
            });


        }
        public void SetChutBar(ChatUpBur bar)
        {
            optimalChat = bar;
        }
        private void StopVoiceAndreciver()
        {
            l.log1("[StopVoiceAndreciver] Начал очистку");
            l.log1($"[StopVoiceAndreciver] был порт {udpClient.Client.LocalEndPoint}");
            l.log1("[StopVoiceAndreciver] закрыл клиент");
            _voiceClient.StopRecording();
            _voiceReceiver.StopListening();
            udpClient.Close();
            CreateNewUdpPort();
            UpdateUdpClients();
            l.log1($"[StopVoiceAndreciver] Пересоздал порт {udpClient.Client.LocalEndPoint}");

        }
        public void CreateNewUdpPort()
        {
            udpClient = new UdpClient(new IPEndPoint(IPAddress.Any, 0));
        }
        public void UpdateUdpClients()
        {
            _voiceClient._udpClient = udpClient;
            _voiceReceiver._udpClient = udpClient;
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
            optimalChat.LoadUserDataInCall(Properties.Settings.Default.UserID);
            _voiceReceiver.StartListening();
            l.log("[VoiceClient] Вызов StartRecording()...");
            _voiceClient.StartRecording();
            l.log("[VoiceClient] Вызвался успешно StartRecording()...");
            soundController.StartAccept();
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
            soundController.StartDenied();
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
            soundController.StartDenied();
            StopVoiceAndreciver();
        }
        public void MuteMicro ()
        {
            if (_voiceClient.volumeClient == 0.0f)
            {
                ///ToDo тут насроку надо сделать
                _voiceClient.volumeClient = 0.5f;
            }
            else
            {
                _voiceClient.volumeClient = 0.0f;
            }
        }
        public void MuteAllCall()
        {
            if (_voiceReceiver.volumeReciver == 0.0f)
            {
                ///ToDo тут насроку надо сделать
                _voiceReceiver.volumeReciver = 0.5f;
                _voiceClient.volumeClient = 0.5f;

            }
            else
            {
                _voiceReceiver.volumeReciver = 0.0f;
                _voiceClient.volumeClient = 0.0f;
            }

        }

    }
}
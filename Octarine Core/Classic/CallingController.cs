using Microsoft.AspNetCore.SignalR.Client;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Octarine_Core.Resource.UsersIntefeces;
using Octarine_Core.Autorisation;
using System.Net.Sockets;
using Octarine_Core.Apis;
using Octarine_Core.Models;
using NAudio.Wave;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Octarine_Core.Classic
{
    public class CallingController : IDisposable
    {
        private HubConnection _connection;
        public OctarineWindow _octarine;
        private VoiceReceiver _voiceReceiver;
        public VoiceClient _voiceClient;
        private bool _isDisposed;
        private Log l = new Log();
        ApiRequests apir;

        public CallingController(OctarineWindow octarineWindow)
        {
            for (int i = 0; i < WaveIn.DeviceCount; i++)
            {
                var capabilities = WaveIn.GetCapabilities(i);
                l.log($"Микрофон {i}: {capabilities.ProductName}");
            }
            _voiceReceiver = new VoiceReceiver();
            _voiceClient = new VoiceClient();
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
            _connection.On<int>("ReceiveUdpPort", (udpPort) =>
            {
                l.log($"[VoiceReceiver] Получил свой реальный UDP-порт от сервера: {udpPort}");
                _voiceReceiver.SetUdpPort(udpPort);
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
            l.log($"[StartCallAsync] Подготовка к вызову. Комната: {request.RoomId}");

            await ResetForNewCall();

            try
            {
                var message = await apir.PostAsync<object>(Properties.Settings.Default.StartCallAPI, request);

                _ = Task.Run(() => _voiceReceiver.StartListening(_voiceClient._udpClient));
                _voiceClient.StartRecording();

                l.log("[StartCallAsync] Вызов успешно начат");
            }
            catch (Exception ex)
            {
                l.Ex($"[StartCallAsync] Ошибка:{ex.Message}");
                throw;
            }
        }

        public async Task AcceptCallAsync(string userId, string roomId)
        {
            l.log($"[AcceptCallAsync] Подготовка к принятию вызова...");

            // Сбрасываем состояние перед принятием звонка
            await ResetForNewCall();

            try
            {
                var CallConfirmation = new
                {
                    RoomId = roomId,
                    UserId = userId,
                };

                var message = await apir.PostAsync<object>(Properties.Settings.Default.ConfirmCallAPI, CallConfirmation);

                _ = Task.Run(() => _voiceReceiver.StartListening(_voiceClient._udpClient));
                _voiceClient.StartRecording();

                l.log("[AcceptCallAsync] Вызов успешно принят");
            }
            catch (Exception ex)
            {
                l.Ex($"[AcceptCallAsync] Ошибка: {ex.Message}");
                throw;
            }
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
        public async Task EndCall (string userId, string roomId)
        {
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
            _voiceClient.StopRecording();

        }
        public async Task ResetForNewCall()
        {
            l.log("[ResetForNewCall] Сброс состояния для нового звонка...");

            try
            {
                // Останавливаем текущую запись
                if (_voiceClient != null)
                {
                    _voiceClient.StopRecording();
                    await Task.Delay(200);
                }

                // Пересоздаем компоненты
                ResetVoiceComponents();

                // Переподключаем SignalR если соединение разорвано
                if (_connection.State != HubConnectionState.Connected)
                {
                    await StartConnectionAsync();
                }
            }
            catch (Exception ex)
            {
                l.Ex($"[ResetForNewCall] Ошибка: {ex.Message}");
            }
        }

        private void ResetVoiceComponents()
        {
            // Освобождаем старые ресурсы
            _voiceReceiver?.Dispose();
            _voiceClient?.Dispose();

            // Создаем новые экземпляры
            _voiceReceiver = new VoiceReceiver();
            _voiceClient = new VoiceClient();
        }
        public void Dispose()
        {
            if (_isDisposed) return;

            _voiceClient?.Dispose();
            _voiceReceiver?.Dispose();
            _connection?.DisposeAsync().GetAwaiter().GetResult();

            _isDisposed = true;
            GC.SuppressFinalize(this);
        }
        ~CallingController() => Dispose();
    }
}

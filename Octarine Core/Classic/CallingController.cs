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
using System.Net;

namespace Octarine_Core.Classic
{
    public class CallingController : IDisposable
    {
        private readonly HubConnection _connection;
        private readonly OctarineWindow _octarine;
        private VoiceReceiver _voiceReceiver;
        private VoiceClient _voiceClient;
        private bool _isDisposed;
        private readonly UdpClient _udpClient;
        private readonly Log l = new Log();
        private readonly ApiRequests apir;

        public CallingController(OctarineWindow octarineWindow)
        {
            for (int i = 0; i < WaveIn.DeviceCount; i++)
            {
                var capabilities = WaveIn.GetCapabilities(i);
                l.log($"Микрофон {i}: {capabilities.ProductName}");
            }

            _udpClient = new UdpClient(new IPEndPoint(IPAddress.Any, 0));
            _voiceReceiver = new VoiceReceiver(_udpClient);
            _voiceClient = new VoiceClient(_udpClient);

            _octarine = octarineWindow;
            apir = new ApiRequests();
            Properties.Settings.Default.UserPort = 0;

            _connection = new HubConnectionBuilder()
                .WithUrl($"http://147.45.175.135:5001/voiceHub?userId={Properties.Settings.Default.UserID}")
                .Build();

            SetupSignalREvents();
        }

        private void SetupSignalREvents()
        {
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

            _connection.On<string, string>("UserJoinedCall", (roomId, userId) =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    l.log($"[UserJoinedCall] User {userId} подключился в комнату {roomId}");
                    foreach (var item in _octarine.IngoGrid.Children)
                    {
                        if (item is ChatUpBur cub)
                            cub._callTimer.Stop();
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
                l.log($"[VoiceReceiver] Получен UDP-порт от сервера: {udpPort}");
                _voiceReceiver.SetUdpPort(udpPort);
            });

            _connection.On<string, string>("RejectEndCall", (roomId, userId) =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    foreach (var item in _octarine.MainGrid.Children)
                    {
                        if (item is EntreredCall ec)
                        {
                            var parentContainer = VisualTreeHelper.GetParent(ec) as Panel;
                            parentContainer?.Children.Remove(ec);
                        }
                    }
                });
            });

            _connection.On<string, string>("UserLeftCall", (roomId, userId) =>
            {
                Application.Current.Dispatcher.Invoke(async () =>
                {
                    foreach (var item in _octarine.MainGrid.Children)
                    {
                        if (item is ChatUpBur cub)
                            await cub.EndCall();
                    }
                });
            });
        }

        public async Task StartConnectionAsync()
        {
            try
            {
                await _connection.StartAsync();
                l.log($"[StartConnectionAsync] Подключено. UserID: {Properties.Settings.Default.UserID}");
            }
            catch (Exception ex)
            {
                l.Ex($"[StartConnectionAsync] Ошибка: {ex.Message}");
            }
        }

        public async Task StartCallAsync(CallRequest request)
        {
            l.log($"[StartCallAsync] Начинаем вызов. Комната: {request.RoomId}");
            await ResetForNewCall();

            try
            {
                await apir.PostAsync<object>(Properties.Settings.Default.StartCallAPI, request);
                _ = Task.Run(_voiceReceiver.StartListening);
                _voiceClient.StartRecording();
                l.log("[StartCallAsync] Вызов начат");
            }
            catch (Exception ex)
            {
                l.Ex($"[StartCallAsync] Ошибка: {ex.Message}");
                throw;
            }
        }

        public async Task AcceptCallAsync(string userId, string roomId)
        {
            l.log("[AcceptCallAsync] Принятие вызова...");
            await ResetForNewCall();

            try
            {
                var payload = new { RoomId = roomId, UserId = userId };
                await apir.PostAsync<object>(Properties.Settings.Default.ConfirmCallAPI, payload);
                _ = Task.Run(_voiceReceiver.StartListening);
                _voiceClient.StartRecording();
                l.log("[AcceptCallAsync] Вызов принят");
            }
            catch (Exception ex)
            {
                l.Ex($"[AcceptCallAsync] Ошибка: {ex.Message}");
                throw;
            }
        }

        public async Task RejectCallAsync(string userId, string roomId)
        {
            l.log($"[RejectCallAsync] Отклонение вызова. User: {userId}, Room: {roomId}");

            try
            {
                var payload = new { RoomId = roomId, UserId = userId };
                await apir.PostAsync<object>(Properties.Settings.Default.RejectCall, payload);
            }
            catch (Exception ex)
            {
                l.Ex($"[RejectCallAsync] Ошибка: {ex.Message}");
            }
        }

        public async Task EndCall(string userId, string roomId)
        {
            l.log($"[EndCall] Завершение вызова. User: {userId}, Room: {roomId}");

            try
            {
                var payload = new { RoomId = roomId, UserId = userId };
                await apir.PostAsync<object>(Properties.Settings.Default.EndCall, payload);
            }
            catch (Exception ex)
            {
                l.Ex($"[EndCall] Ошибка: {ex.Message}");
            }

            _voiceClient.StopRecording();
        }

        public async Task ResetForNewCall()
        {
            l.log("[ResetForNewCall] Сброс состояния...");

            try
            {
                _voiceClient?.StopRecording();
                await Task.Delay(200);
                ResetVoiceComponents();

                if (_connection.State != HubConnectionState.Connected)
                    await StartConnectionAsync();
            }
            catch (Exception ex)
            {
                l.Ex($"[ResetForNewCall] Ошибка: {ex.Message}");
            }
        }

        private void ResetVoiceComponents()
        {
            _voiceReceiver?.Dispose();
            _voiceClient?.Dispose();
            _voiceReceiver = new VoiceReceiver(_udpClient);
            _voiceClient = new VoiceClient(_udpClient);
        }

        public void Dispose()
        {
            if (_isDisposed) return;

            _voiceClient?.Dispose();
            _voiceReceiver?.Dispose();
            _connection?.DisposeAsync().GetAwaiter().GetResult();
            _udpClient?.Dispose();

            _isDisposed = true;
            GC.SuppressFinalize(this);
        }

        ~CallingController() => Dispose();
    }
}

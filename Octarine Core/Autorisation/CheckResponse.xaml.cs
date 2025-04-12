using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Octarine_Core.Classic;


namespace Octarine_Core.Autorisation
{
    /// <summary>
    /// Логика взаимодействия для CheckResponse.xaml
    /// </summary>
    public partial class CheckResponse : Window
    {
        private class ResponseAPi
        {
            [JsonPropertyName("status")]
            public string StatisDataBase { get; set; }
            [JsonPropertyName("responseTimeMs")]
            public long TimeResponse { get; set; }
        }

        private readonly int _maxRetries = 3;
        private readonly int _retryDelayMs = 1000;
        private bool _allServicesAvailable = false;

        public CheckResponse()
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.Show();
            PingAll();

            Properties.Settings.Default.JwtToken = null;
        }

        private async void PingAll()
        {
            var tasks = new List<Task<bool>>
        {
            CheckServiceWithRetryAsync("API", PingHttpApiAsync, ApiTB),
            CheckServiceWithRetryAsync("WebSocket", PingWebSocketAsync, WebSocketTB),
            CheckServiceWithRetryAsync("MinIO", () => new MinIO().PingMinioAsync(), MinIoTB)
        };

            var results = await Task.WhenAll(tasks);
            _allServicesAvailable = results.All(x => x);

            if (_allServicesAvailable)
            {
                OnAllServicesAvailable();
            }
            else
            {
                StatusTB.Text = "Некоторые сервисы недоступны после нескольких попыток";
            }
        }

        private async Task<bool> CheckServiceWithRetryAsync(string serviceName, Func<Task<long>> pingFunc, TextBlock statusTextBlock)
        {
            int attempt = 0;
            long responseTime = -1;

            while (attempt < _maxRetries)
            {
                attempt++;
                responseTime = await pingFunc();

                if (responseTime >= 0)
                {
                    statusTextBlock.Text = $"{responseTime}ms";
                    return true;
                }

                statusTextBlock.Text = $"Попытка {attempt} из {_maxRetries}...";
                await Task.Delay(_retryDelayMs);
            }

            statusTextBlock.Text = "Недоступен";
            return false;
        }

        private async void OnAllServicesAvailable()
        {
            StatusTB.Text = "Успех";
            await Task.Delay(1000);

            Window windowToShow;

            if (Properties.Settings.Default.JwtToken != null)
            {
                StatusTB.Text = "Загрузка главного окна...";
                windowToShow = new OctarineWindow();
            }
            else
            {
                StatusTB.Text = "Загрузка окна авторизации...";
                windowToShow = new MainWindow();
            }

            windowToShow.Loaded += (sender, e) =>
            {
                this.Close();
            };

            windowToShow.Show();

            if (windowToShow.IsLoaded)
            {
                this.Close();
            }
        }

        public async Task<long> PingHttpApiAsync()
        {
            var stopwatch = Stopwatch.StartNew();

            try
            {
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.GetAsync(Properties.Settings.Default.PingApi);
                    var responseContent = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        var answer = JsonSerializer.Deserialize<ResponseAPi>(responseContent);
                        return answer?.TimeResponse ?? stopwatch.ElapsedMilliseconds;
                    }
                    else
                    {
                        try
                        {
                            var responseData = JsonSerializer.Deserialize<Dictionary<string, string>>(responseContent);
                            if (responseData != null && responseData.ContainsKey("message"))
                            {
                                throw new Exception(responseData["message"]);
                            }
                            throw new Exception("Произошла непредвиденная ошибка сервера.");
                        }
                        catch
                        {
                            return -1;
                        }
                    }
                }
            }
            catch
            {
                return -1;
            }
        }

        public async Task<long> PingWebSocketAsync()
        {
            try
            {
                var chathub = new ChatHub(null, null);
                return await chathub.PingApi();
            }
            catch
            {
                return -1;
            }
        }
    }
}


using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Octarine_Core.Apis
{
    internal class ApiRequests
    {
        public async Task<string> PostAsync<TRequest>(string apiUrl, TRequest request)
        {
            if (string.IsNullOrEmpty(apiUrl))
                throw new ArgumentException("URL не может быть пустым.", nameof(apiUrl));
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.PostAsync(apiUrl, content);

                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    return responseContent;  // Возвращаем строку, если запрос успешен
                }
                else
                {
                    // В случае ошибки десериализуем в Dictionary для извлечения сообщения
                    var responseData = JsonSerializer.Deserialize<Dictionary<string, string>>(responseContent);
                    if (responseData != null && responseData.ContainsKey("message"))
                    {
                        throw new Exception(responseData["message"]);
                    }
                    throw new Exception("Произошла непредвиденная ошибка сервера.");
                }
            }
        }

    }
}

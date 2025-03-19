using System.Threading.Tasks;
using System.Windows;
using System;
using System.Net.Http;
using System.IO;
using System.Windows.Media.Imaging;

namespace Octarine_Core.Classic
{
    public class MinIO
    {
        private static string mainUrl = "http://147.45.175.135:9000/avatars/";

        public async Task<BitmapImage> LoadImageFromMinIO(string imageName)
        {
            string imageUrl = mainUrl + imageName;
            var imageBytes = await GetImageBytes(imageUrl);
            if (imageBytes != null)
            {
                using (var memoryStream = new MemoryStream(imageBytes))
                {
                    var bitmapImage = new BitmapImage();
                    bitmapImage.BeginInit();
                    bitmapImage.StreamSource = memoryStream;
                    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                    bitmapImage.EndInit();

                    return bitmapImage; 
                }
            }

            return null;
        }

        private async Task<byte[]> GetImageBytes(string url)
        {
            try
            {
                HttpClient client = new HttpClient();
                var response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsByteArrayAsync();
                }
                else
                {
                    MessageBox.Show("Ошибка загрузки изображения");
                    return null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
                return null;
            }
        }
    }
}

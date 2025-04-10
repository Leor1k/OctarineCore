using Minio;
using Minio.DataModel;
using Microsoft.Win32;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Minio.DataModel.Args;

namespace Octarine_Core.Classic
{
    public class MinIO
    {
        private static string mainUrl = "http://147.45.175.135:9000/avatars/";
        private static string endpoint = "147.45.175.135:9000";
        private static string accessKey = "admin"; 
        private static string secretKey = "admin123"; 
        private static string bucketName = "avatars";

        private IMinioClient minioClient = new MinioClient()
            .WithEndpoint(endpoint)
            .WithCredentials(accessKey, secretKey)
            .WithSSL(false)
            .Build();

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
                    return await response.Content.ReadAsByteArrayAsync();
                return null;
            }
            catch
            {
                return null;
            }
        }

        public async Task UploadUserAvatar(string userId)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Изображения (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg";

            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;
                string objectName = $"IconUser{userId}.png";

                try
                {
                    await minioClient.StatObjectAsync(new StatObjectArgs()
                        .WithBucket(bucketName)
                        .WithObject(objectName));

                    await minioClient.RemoveObjectAsync(new RemoveObjectArgs()
                        .WithBucket(bucketName)
                        .WithObject(objectName));
                }
                catch
                {
                    // ничего страшного, если её не было
                }

                // Загружаем новую
                await minioClient.PutObjectAsync(new PutObjectArgs()
                    .WithBucket(bucketName)
                    .WithObject(objectName)
                    .WithFileName(filePath)
                    .WithContentType("image/png")); // или определять тип динамически
            }
        }
    }
}

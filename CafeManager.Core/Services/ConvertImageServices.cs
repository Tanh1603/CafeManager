using System.IO;
using System.Windows.Media.Imaging;

namespace CafeManager.Core.Services
{
    public static class ConvertImageServices
    {
        public static byte[] BitmapImageToByteArray(BitmapImage? bitmapImage)
        {
            if (bitmapImage == null)
            {
                return [];
            }
            byte[] data;
            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bitmapImage));

            using (var memoryStream = new MemoryStream())
            {
                encoder.Save(memoryStream);
                data = memoryStream.ToArray();
            }

            return data;
        }

        public static BitmapImage? ByteArrayToBitmapImage(byte[] byteArray)
        {
            if (byteArray.Length == 0)
            {
                return null;
            }
            var bitmapImage = new BitmapImage();
            using (MemoryStream memoryStream = new(byteArray))
            {
                memoryStream.Position = 0; // Đặt lại con trỏ
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memoryStream;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad; // Tải toàn bộ dữ liệu vào bộ nhớ
                bitmapImage.EndInit();
                bitmapImage.Freeze(); // Đóng băng để tránh thay đổi
            }
            return bitmapImage;
        }
    }
}
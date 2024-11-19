using System.IO;
using System.Windows.Media.Imaging;

namespace CafeManager.Core.Services
{
    public static class ConvertImageServices
    {
        public static string BitmapImageToBase64(BitmapImage? bitmapImage)
        {
            if (bitmapImage == null)
                return string.Empty;

            // Convert BitmapImage to a MemoryStream
            using (var memoryStream = new MemoryStream())
            {
                BitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bitmapImage));
                encoder.Save(memoryStream);
                byte[] imageBytes = memoryStream.ToArray();

                // Convert byte array to base64 string
                return Convert.ToBase64String(imageBytes);
            }
        }

        public static BitmapImage? Base64ToBitmapImage(string base64String)
        {
            if (string.IsNullOrEmpty(base64String))
            {
                return null;
            }
            byte[] imageBytes = Convert.FromBase64String(base64String);
            using (var stream = new MemoryStream(imageBytes))
            {
                BitmapImage image = new BitmapImage();
                image.BeginInit();
                image.StreamSource = stream;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.EndInit();
                image.Freeze();
                return image;
            }
        }
    }
}
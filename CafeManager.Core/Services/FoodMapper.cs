using CafeManager.Core.Data;
using CafeManager.Core.DTOs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace CafeManager.Core.Services
{
    public static class FoodMapper
    {
        private static string ConvertBitmapImageToBase64(BitmapImage bitmapImage)
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

        private static BitmapImage? Base64ToBitmapImage(string base64String)
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

        public static FoodDTO ToDTO(Food food)
        {
            return new FoodDTO()
            {
                Foodid = food.Foodid,
                Foodname = food.Foodname,
                Foodcategoryid = food.Foodcategoryid,
                Price = food.Price ?? 0,
                Imagefood = Base64ToBitmapImage(food.Imagefood),
                Discountfood = food.Discountfood ?? 0,
                Isdeleted = food.Isdeleted,
            };
        }

        public static Food ToEntity(FoodDTO foodDTO)
        {
            return new Food()
            {
                Foodid = foodDTO.Foodid,
                Foodname = foodDTO.Foodname,
                Foodcategoryid = foodDTO.Foodcategoryid,
                Price = foodDTO.Price,
                Imagefood = ConvertBitmapImageToBase64(foodDTO.Imagefood),
                Discountfood = foodDTO.Discountfood,
                Isdeleted = foodDTO.Isdeleted,
            };
        }
    }
}
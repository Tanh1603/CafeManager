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
        public static FoodDTO ToDTO(Food food)
        {
            return new FoodDTO()
            {
                Foodid = food.Foodid,
                Foodname = food.Foodname,
                Foodcategoryid = food.Foodcategoryid,
                Price = food.Price ?? 0,
                Imagefood = ConvertImageServices.Base64ToBitmapImage(food.Imagefood),
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
                Imagefood = ConvertImageServices.BitmapImageToBase64(foodDTO.Imagefood),
                Discountfood = foodDTO.Discountfood,
                Isdeleted = foodDTO.Isdeleted,
            };
        }
    }
}
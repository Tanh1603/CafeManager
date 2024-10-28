using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeManager.Core.DTOs
{
    public class FoodDTO
    {
        //public FoodDTO(string displayFoodName, decimal? foodPrice, decimal? discountFood, string imageFood)
        //{
        //    DisplayFoodName = displayFoodName;
        //    FoodPrice = foodPrice;
        //    DiscountFood = discountFood;
        //    ImageFood = imageFood;
        //}

        public string DisplayFoodName { get; set; }
        public decimal? FoodPrice { get; set; }
        public decimal? DiscountFood { get; set; }
        public string ImageFood { get; set; }
        public decimal? PriceDiscount => FoodPrice * DiscountFood;
    }
}
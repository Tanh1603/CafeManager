using CafeManager.Core.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace CafeManager.Core.DTOs
{
    public class FoodDTO
    {
        public int Id { get; set; }
        public string Foodname { get; set; }
        public decimal? Price { get; set; }
        public decimal? Discountfood { get; set; }
        public BitmapImage? Imagefood { get; set; }
        public Foodcategory Foodcategory { get; set; }

        [NotMapped]
        public decimal? PriceDiscount => Price * Discountfood;
    }
}
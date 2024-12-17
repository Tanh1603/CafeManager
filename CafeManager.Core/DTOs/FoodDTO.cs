using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media.Imaging;
using System.ComponentModel.DataAnnotations;
using System.Windows.Controls;
using CommunityToolkit.Mvvm.ComponentModel;

#nullable disable

namespace CafeManager.Core.DTOs
{
    public partial class FoodDTO : BaseDTO
    {
        [ObservableProperty]
        private int _foodid;

        [ObservableProperty]
        private string _foodname;

        [ObservableProperty]
        private int _foodcategoryid;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(PriceDiscount))]
        private decimal _price;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(PriceDiscount))]
        private BitmapImage? _imagefood;

        [ObservableProperty]
        private decimal _discountfood;

        [ObservableProperty]
        private FoodCategoryDTO _foodcategory;

        public decimal? PriceDiscount => Price * (100 - Discountfood) / 100;

        public FoodDTO Clone()
        {
            return new FoodDTO()
            {
                Id = Id,
                Foodid = Foodid,
                Foodname = Foodname,
                Foodcategoryid = Foodcategoryid,
                Price = Price,
                Imagefood = Imagefood,
                Discountfood = Discountfood,
                Isdeleted = Isdeleted,
            };
        }
    }
}
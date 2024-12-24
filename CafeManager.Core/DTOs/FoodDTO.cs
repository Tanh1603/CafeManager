using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Windows.Media.Imaging;

#nullable disable

namespace CafeManager.Core.DTOs
{
    public partial class FoodDTO : BaseDTO
    {
        [ObservableProperty]
        private int _foodid;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessage = "Không được trống")]
        private string _foodname;

        [ObservableProperty]
        private int _foodcategoryid;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessage = "Không được trống")]
        [Range(0, double.MaxValue, ErrorMessage = "Giá phải lớn hơn 0")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Giá phải là số")]
        [NotifyPropertyChangedFor(nameof(PriceDiscount))]
        private decimal _price;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(PriceDiscount))]
        private BitmapImage? _imagefood;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessage = "Không được trống")]
        [Range(0, 100, ErrorMessage = "Giảm giá phải trong khoảng từ 0 đến 100")]
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
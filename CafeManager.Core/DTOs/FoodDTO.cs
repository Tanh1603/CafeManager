using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media.Imaging;

#nullable disable

namespace CafeManager.Core.DTOs
{
    public class FoodDTO : BaseDTO
    {
        private int _foodid;

        private string _foodname;

        private int _foodcategoryid;

        private decimal _price;

        private BitmapImage _imagefood;

        private decimal _discountfood;

        private bool? _isdeleted;

        private FoodCategoryDTO _foodcategory;
        public decimal? PriceDiscount => Price * (100 - Discountfood) / 100;

        public int Foodid
        {
            get => _foodid;
            set
            {
                if (_foodid != value)
                {
                    _foodid = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Foodname
        {
            get => _foodname;
            set
            {
                if (_foodname != value)
                {
                    _foodname = value;
                    OnPropertyChanged();
                }
            }
        }

        public int Foodcategoryid
        {
            get => _foodcategoryid;
            set
            {
                if (_foodcategoryid != value)
                {
                    _foodcategoryid = value;
                    OnPropertyChanged();
                }
            }
        }

        public decimal Price
        {
            get => _price;
            set
            {
                if (_price != value)
                {
                    _price = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(PriceDiscount));
                }
            }
        }

        public BitmapImage Imagefood
        {
            get => _imagefood;
            set
            {
                if (_imagefood != value)
                {
                    _imagefood = value;
                    OnPropertyChanged();
                }
            }
        }

        public decimal Discountfood
        {
            get => _discountfood;
            set
            {
                if (_discountfood != value)
                {
                    _discountfood = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(PriceDiscount));
                }
            }
        }

        public bool? Isdeleted
        {
            get => _isdeleted;
            set
            {
                if (_isdeleted != value)
                {
                    _isdeleted = value;
                    OnPropertyChanged();
                }
            }
        }

        public FoodCategoryDTO Foodcategory
        {
            get => _foodcategory;
            set
            {
                if (_foodcategory != value)
                {
                    _foodcategory = value;
                    OnPropertyChanged();
                }
            }
        }

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
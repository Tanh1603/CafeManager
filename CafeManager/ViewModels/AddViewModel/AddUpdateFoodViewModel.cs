using CafeManager.Core.Data;
using CafeManager.Core.DTOs;
using CafeManager.WPF.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.Windows.Media.Imaging;

namespace CafeManager.WPF.ViewModels.AddViewModel
{
    public partial class AddUpdateFoodViewModel : ObservableObject
    {
        private readonly IServiceProvider _provider;
        private readonly FileDialogService _fileDialogService;

        [ObservableProperty]
        private int _id;

        [ObservableProperty]
        private string _foodname;

        [ObservableProperty]
        private ObservableCollection<Foodcategory> _listFoodCategory;

        [ObservableProperty]
        private string _price;

        [ObservableProperty]
        private BitmapImage _imagefood;

        [ObservableProperty]
        private string _discountFood;

        [ObservableProperty]
        private Foodcategory _selectedFoodCategory;

        private string imageString;
        public bool IsUpdating { get; set; } = false;
        public bool IsAdding { get; set; } = false;

        public event Action<Food> AddFoodChanged;

        public event Action<Food> UpdateFoodChanged;

        public event Action CloseVM;

        public AddUpdateFoodViewModel(IServiceProvider provider)
        {
            _provider = provider;
            _fileDialogService = _provider.GetRequiredService<FileDialogService>();
        }

        public void ReceiveFood(FoodDTO food)
        {
            Foodname = food.Foodname;
            Price = food.Price.ToString() ?? string.Empty;
            Imagefood = food?.Imagefood;
            SelectedFoodCategory = food.Foodcategory;
            DiscountFood = food.Discountfood.ToString() ?? food.Price.ToString() ?? string.Empty;
            Id = food.Id;
        }

        public void ReceiveListFoodCategory(List<Foodcategory> foodcategories)
        {
            ListFoodCategory = new(foodcategories);
        }

        public void ClearValueOfForm()
        {
            this.Foodname = string.Empty;
            this.Price = string.Empty;
            this.Imagefood = null;
            this.SelectedFoodCategory = null;
            this.DiscountFood = string.Empty;
        }

        [RelayCommand]
        private void Submit()
        {
            Food food = new()
            {
                Foodname = this.Foodname ?? string.Empty,
                Price = decimal.TryParse(this.Price, out var price) ? price : 0m,
                Imagefood = this.Imagefood != null ? _fileDialogService.ConvertBitmapImageToBase64((BitmapImage)this.Imagefood) : string.Empty,
                Foodcategoryid = this.SelectedFoodCategory?.Foodcategoryid ?? 0,
                Discountfood = decimal.TryParse(this.DiscountFood, out var discount) ? discount : 0m,
            };

            if (IsAdding)
            {
                AddFoodChanged?.Invoke(food);
            }
            if (IsUpdating)
            {
                food.Foodid = Id;
                UpdateFoodChanged?.Invoke(food);
            }
        }

        [RelayCommand]
        private void Close()
        {
            CloseVM?.Invoke();
            this.Foodname = string.Empty;
            this.Price = string.Empty;
            this.Imagefood = null;
            this.SelectedFoodCategory = null;
            this.DiscountFood = string.Empty;

            IsAdding = false;
            IsUpdating = false;
        }

        [RelayCommand]
        private void OpenUploadImage()
        {
            string filter = "Image files (*.png;*.jpg;*.jpeg;*.bmp)|*.png;*.jpg;*.jpeg;*.bmp|All files (*.*)|*.*";
            var filePath = _fileDialogService.OpenFileDialog(filter);

            if (!string.IsNullOrEmpty(filePath))
            {
                Imagefood = new BitmapImage(new Uri(filePath));
            }
        }
    }
}
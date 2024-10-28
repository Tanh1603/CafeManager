using CafeManager.Core.Data;
using CafeManager.Core.DTOs;
using CafeManager.Core.Services;
using CafeManager.Infrastructure.Models;
using CafeManager.WPF.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeManager.WPF.ViewModels
{
    public partial class FoodViewModel : ObservableObject
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly FoodServices _foodServices;

        [ObservableProperty]
        private ObservableCollection<Foodcategory> _foodCategoryList;

        [ObservableProperty]
        private ObservableCollection<FoodDTO> _foodList;

        [ObservableProperty]
        private bool _isOpenAddNewFood;

        public FoodViewModel(IServiceProvider provider)
        {
            _serviceProvider = provider;
            _foodServices = provider.GetRequiredService<FoodServices>();
            LoadData();
        }

        private async void LoadData()
        {
            FoodCategoryList = new ObservableCollection<Foodcategory>(await _foodServices.GetAllFoodCategoryAsync());
            FoodList = new ObservableCollection<FoodDTO>(await _foodServices.GetAllFoodByCategoryIdAsync(FoodCategoryList[0].Foodcategoryid));
        }

        [RelayCommand]
        private async void ChangeFoodList(int foodCategoryId)
        {
            FoodList = new ObservableCollection<FoodDTO>(await _foodServices.GetAllFoodByCategoryIdAsync(foodCategoryId));
        }

        [RelayCommand]
        private async void AddFoodCategory()
        {
            Foodcategory foodcategory = new Foodcategory()
            {
                Displayname = "test food1"
            };
            var rest = _foodServices.AddNewFoodCategory(foodcategory);
            OnPropertyChanged(nameof(FoodCategoryList));
        }

        [RelayCommand]
        private async void UpdateFoodCategory()
        {
            Foodcategory obj = new Foodcategory()
            {
                Foodcategoryid = 11,
                Displayname = "Test update1"
            };

            await _foodServices.UpdatNewFoodCategory(obj);
        }

        [RelayCommand]
        private async void DeleteFoodCategory()
        {
            await _foodServices.DeleteFoodCategory(10);
        }

        [RelayCommand]
        private async void AddFood()
        {
            Food food = new Food()
            {
                Displayname = "test food1",
                Foodcategoryid = 11,
                Price = 2000,
                Imagefood = "trống",
                Discountfood = 10,
            };
            var rest = _foodServices.AddNewFood(food);
        }

        [RelayCommand]
        private async void UpdateFood()
        {
            Food obj = new Food()
            {
                Foodid = 15,
                Displayname = "Test food",
                Foodcategoryid = 10,
                Price = 9999,
                Imagefood = "trống",
                Discountfood = 999,
            };

            await _foodServices.UpdatNewFood(obj);
        }

        [RelayCommand]
        private async void DeleteFood()
        {
            await _foodServices.DeletFood(15);
        }
    }
}
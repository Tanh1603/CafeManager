using CafeManager.Core.Data;
using CafeManager.Core.DTOs;
using CafeManager.WPF.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace CafeManager.WPF.ViewModels
{
    public partial class SupplierViewModel : ObservableObject
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly FoodCategoryServices _foodCategoryServices;
        private readonly FoodServices _foodServices;

        [ObservableProperty]
        private ObservableCollection<Foodcategory> _foodCategoryList = new();

        [ObservableProperty]
        private ObservableCollection<Food> _foodListByFoodCategoryId = new();

        [ObservableProperty]
        private Foodcategory _selectedFoodCategory = new();

        public SupplierViewModel(IServiceProvider provider)
        {
            _serviceProvider = provider;
            _foodCategoryServices = provider.GetRequiredService<FoodCategoryServices>();
            _foodServices = provider.GetRequiredService<FoodServices>();

            LoadData();
        }

        private async Task LoadData()
        {
            FoodCategoryList = new ObservableCollection<Foodcategory>(await _foodCategoryServices.GetListCategory());
            SelectedFoodCategory = FoodCategoryList[0];
            FoodListByFoodCategoryId = new ObservableCollection<Food>(await _foodCategoryServices.GetListFoodByFoodCatgoryId(SelectedFoodCategory.Foodcategoryid));
        }

        [RelayCommand]
        private async Task ChangeFoodCategory(Foodcategory foodcategory)
        {
            FoodListByFoodCategoryId = new ObservableCollection<Food>(
                await _foodCategoryServices.GetListFoodByFoodCatgoryId(foodcategory.Foodcategoryid));
        }

        [RelayCommand]
        private async Task AddFoodCategory()
        {
            var newfc = new Foodcategory()
            {
                Foodcategoryname = "Test fc 1"
            };
            await _foodCategoryServices.AddFoodCategory(newfc);

            FoodCategoryList.Add(newfc);
        }

        [RelayCommand]
        private void UpdateFoodCategory()
        {
            FoodCategoryList[4].Foodcategoryname = "Test update 1";
            _foodCategoryServices.UpdateFoodCategory(FoodCategoryList[4]);
            FoodCategoryList = new ObservableCollection<Foodcategory>(FoodCategoryList);
        }

        [RelayCommand]
        private async Task DeleteFoodCategory()
        {
            await _foodCategoryServices.DeleteFoodCategory(1);
            FoodCategoryList.Remove(FoodCategoryList[0]);
            FoodListByFoodCategoryId.Clear();
            FoodListByFoodCategoryId = new(FoodListByFoodCategoryId = new ObservableCollection<Food>(await _foodCategoryServices.GetListFoodByFoodCatgoryId(2)));
        }

        // add food
        [RelayCommand]
        private async Task AddFood()
        {
            var newfc = new Food()
            {
                Foodname = "Test f 1",
                Price = 10000,
                Discountfood = 10,
                Imagefood = "Chưa có",
                Foodcategoryid = 5
            };
            await _foodServices.AddFood(newfc);
        }

        [RelayCommand]
        private void UpdateFood()
        {
            var temp = FoodListByFoodCategoryId.FirstOrDefault(x => x.Foodid == 10);
            if (temp != null)
            {
                temp.Foodname = "test f update";
            }
            _foodServices.UpdatFood(temp);
            FoodCategoryList = new ObservableCollection<Foodcategory>(FoodCategoryList);
        }

        [RelayCommand]
        private async Task DeleteFood()
        {
            await _foodServices.DeletFood(10);
        }
    }
}
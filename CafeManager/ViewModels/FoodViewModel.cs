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
using System.Text;
using System.Threading.Tasks;

namespace CafeManager.WPF.ViewModels
{
    public partial class FoodViewModel : ObservableObject
    {
        private readonly IServiceProvider _serviceProvider;

        [ObservableProperty]
        private ObservableCollection<Foodcategory> _foodCategoryList;

        [ObservableProperty]
        private ObservableCollection<FoodDTO> _foodList;

        [ObservableProperty]
        private bool _isOpenAddNewFood;

        public FoodViewModel(IServiceProvider provider)
        {
            _serviceProvider = provider;
            LoadData();
        }

        private async void LoadData()
        {
            FoodServices fv = _serviceProvider.GetRequiredService<FoodServices>();
            FoodCategoryList = new ObservableCollection<Foodcategory>(await fv.GetAllFoodCategoryAsync());
            FoodList = new ObservableCollection<FoodDTO>(await fv.GetAllFoodByCategoryIdAsync(FoodCategoryList[0].Foodcategoryid));
        }

        [RelayCommand]
        private async void ChangeFoodList(int foodCategoryId)
        {
            FoodList = new ObservableCollection<FoodDTO>
                (await _serviceProvider.GetRequiredService<FoodServices>().GetAllFoodByCategoryIdAsync(foodCategoryId));
        }

        [RelayCommand]
        private void OpenAddNewFood() => IsOpenAddNewFood = true;

        [RelayCommand]
        private void CloseAddNewFood() => IsOpenAddNewFood = false;
    }
}
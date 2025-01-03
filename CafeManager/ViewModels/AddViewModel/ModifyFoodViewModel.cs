﻿using CafeManager.Core.Data;
using CafeManager.Core.DTOs;
using CafeManager.WPF.MessageBox;
using CafeManager.WPF.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Windows.Media.Imaging;

namespace CafeManager.WPF.ViewModels.AddViewModel
{
    public partial class ModifyFoodViewModel : ObservableValidator
    {
        private readonly IServiceProvider _provider;
        private readonly FileDialogService _fileDialogService;

        [ObservableProperty]
        public FoodDTO _modifyFood = new();

        [ObservableProperty]
        private FoodCategoryDTO? _selectedFoodCategory;

        [ObservableProperty]
        private ObservableCollection<FoodCategoryDTO> _listFoodCategory = [];

        public bool IsUpdating { get; set; } = false;
        public bool IsAdding { get; set; } = false;

        public event Action<FoodDTO>? ModifyFoodChanged;

        public ModifyFoodViewModel(IServiceProvider provider)
        {
            _provider = provider;
            _fileDialogService = _provider.GetRequiredService<FileDialogService>();
        }

        public void ReceiveFood(FoodDTO foodDTO)
        {
            ModifyFood = foodDTO.Clone();
            SelectedFoodCategory = ListFoodCategory.FirstOrDefault(x => x.Foodcategoryid == foodDTO.Foodcategoryid);
        }

        public void ReceiveListFoodCategory(List<FoodCategoryDTO> foodcategories) => ListFoodCategory = [.. foodcategories];

        public void ClearValueOfForm()
        {
            ModifyFood = new();
            SelectedFoodCategory = null;
            IsAdding = false;
            IsUpdating = false;
        }

        [RelayCommand]
        private void Submit()
        {
            ModifyFood.ValidateDTO();
            if (ModifyFood.HasErrors)
            {
                return;
            }
            if (SelectedFoodCategory != null && SelectedFoodCategory.Foodcategoryid != 0)
            {
                ModifyFood.Foodcategoryid = SelectedFoodCategory.Foodcategoryid;
                ModifyFoodChanged?.Invoke(ModifyFood.Clone());
            }
            else
            {
                MyMessageBox.ShowDialog("Vui lòng chọn danh mục", MyMessageBox.Buttons.OK, MyMessageBox.Icons.Error);
            }
        }

        [RelayCommand]
        private void OpenUploadImage()
        {
            string filter = "Image files (*.png;*.jpg;*.jpeg;*.bmp)|*.png;*.jpg;*.jpeg;*.bmp|All files (*.*)|*.*";
            var filePath = _fileDialogService.OpenFileDialog(filter);

            if (!string.IsNullOrEmpty(filePath))
            {
                ModifyFood.Imagefood = new BitmapImage(new Uri(filePath));
            }
        }
    }
}
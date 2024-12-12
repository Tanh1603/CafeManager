﻿using AutoMapper;
using CafeManager.Core.Data;
using CafeManager.Core.DTOs;
using CafeManager.Core.Services;
using CafeManager.WPF.MessageBox;
using CafeManager.WPF.Services;
using CafeManager.WPF.ViewModels.AddViewModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace CafeManager.WPF.ViewModels.AdminViewModel
{
    public partial class FoodCategoryViewModel : ObservableObject, IDisposable
    {
        private readonly FoodCategoryServices _foodcategoryServices;
        private IMapper _mapper;
        private readonly IServiceScope _scope;

        [ObservableProperty]
        private bool _isAdding = false;

        [ObservableProperty]
        private bool _isUpdating = false;

        [ObservableProperty]
        private bool _isOpenModifyFoodCategory = false;

        [ObservableProperty]
        private FoodCategoryDTO _modifyFoodCategory = new();

        [ObservableProperty]
        private ObservableCollection<FoodCategoryDTO> _listFoodCategory = [];

        [ObservableProperty]
        private ObservableCollection<FoodCategoryDTO> _listDeletedFoodCategory = [];

        public FoodCategoryViewModel(IServiceScope scope)
        {
            _scope = scope;
            var provider = scope.ServiceProvider;
            _foodcategoryServices = provider.GetRequiredService<FoodCategoryServices>();
            _mapper = provider.GetRequiredService<IMapper>();
        }
        [RelayCommand]
        private void OpenAddFoodCategory()
        {
            IsOpenModifyFoodCategory = true;
            IsAdding = true;
        }

        [RelayCommand]
        private void OpenUpdateFoodCategory(FoodCategoryDTO foodCategory)
        {
            IsOpenModifyFoodCategory = true;
            IsUpdating = true;
            ModifyFoodCategory = foodCategory.Clone();
        }

        [RelayCommand]
        private async Task SubmitModifyFoodCategory()
        {
            try
            {
                if (IsAdding)
                {
                    var addFoodCategory = await _foodcategoryServices.AddFoodCategory(_mapper.Map<Foodcategory>(ModifyFoodCategory));
                    if (addFoodCategory != null)
                    {
                        ListFoodCategory.Add(_mapper.Map<FoodCategoryDTO>(addFoodCategory));
                        MyMessageBox.ShowDialog("Thêm danh mục thực đơn thành công");
                        ModifyFoodCategory = new();
                    }
                    else
                    {
                        MyMessageBox.Show("Thêm dnah mục thực đơn cấp thất bại");
                    }
                }
                if (IsUpdating)
                {
                    var res = await _foodcategoryServices.UpdateFoodCategory(_mapper.Map<Foodcategory>(ModifyFoodCategory));
                    if (res != null)
                    {
                        var updateFoodCategoryDTO = ListFoodCategory.FirstOrDefault(x => x.Foodcategoryid == res.Foodcategoryid);
                        if (updateFoodCategoryDTO != null)
                        {
                            _mapper.Map(res, updateFoodCategoryDTO);
                            MyMessageBox.ShowDialog("Sửa danh mục thực đơn thành công");
                            ModifyFoodCategory = new();
                        }
                    }
                    else
                    {
                        MyMessageBox.ShowDialog("Sửa danh mục thực đơn thất bại");
                    }
                }
                CloseModifyFoodCategory();
            }
            catch (InvalidOperationException ioe)
            {
                MyMessageBox.ShowDialog(ioe.Message);
            }
        }

        [RelayCommand]
        private async Task DeleteFoodCategory(FoodCategoryDTO foodCategory)
        {
            try
            {
                string messageBox = MyMessageBox.ShowDialog("Bạn có muốn ẩn danh mục không?", MyMessageBox.Buttons.Yes_No, MyMessageBox.Icons.Question);
                if (messageBox.Equals("1"))
                {
                    bool isDeleted = await _foodcategoryServices.DeleteFoodCategory(foodCategory.Foodcategoryid);
                    if (isDeleted)
                    {
                        ListDeletedFoodCategory.Add(foodCategory);
                        ListFoodCategory.Remove(foodCategory);
                        MyMessageBox.ShowDialog("Ẩn danh mục thanh công");
                    }
                    else
                    {
                        MyMessageBox.ShowDialog("Ẩn danh mục thất bại");
                    }
                }
            }
            catch (InvalidOperationException ioe)
            {
                MyMessageBox.ShowDialog(ioe.Message);
            }
        }

        [RelayCommand]
        private async Task RestoreFoodCategory(FoodCategoryDTO foodCategory)
        {
            try
            {
                string messageBox = MyMessageBox.ShowDialog("Bạn có muốn hiển thị danh mục không?", MyMessageBox.Buttons.Yes_No, MyMessageBox.Icons.Question);
                if (messageBox.Equals("1"))
                {
                    foodCategory.Isdeleted = false;
                    var res = await _foodcategoryServices.UpdateFoodCategory(_mapper.Map<Foodcategory>(foodCategory));
                    if (res != null)
                    {
                        ListFoodCategory.Add(_mapper.Map<FoodCategoryDTO>(res));
                        ListDeletedFoodCategory.Remove(foodCategory);
                        MyMessageBox.ShowDialog("Hiển thị danh mục thanh công");
                    }
                    else
                    {
                        MyMessageBox.ShowDialog("Hiển thị danh mục thất bại");
                    }
                }
            }
            catch (InvalidOperationException ioe)
            {
                MyMessageBox.ShowDialog(ioe.Message);
            }
        }

        [RelayCommand]
        private void CloseModifyFoodCategory()
        {
            IsOpenModifyFoodCategory = false;
            IsAdding = false;
            IsUpdating= false;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
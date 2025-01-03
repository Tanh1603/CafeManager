using AutoMapper;
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
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace CafeManager.WPF.ViewModels.AdminViewModel
{
    public partial class FoodCategoryViewModel : ObservableValidator, IDisposable
    {
        private readonly FoodCategoryServices _foodcategoryServices;
        private IMapper _mapper;
        private readonly IServiceScope _scope;

        [ObservableProperty]
        private bool _isLoading;

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
                IsLoading = true;
                ModifyFoodCategory.ValidateDTO();
                if (ModifyFoodCategory.HasErrors)
                {
                    return;
                }
                if (IsAdding)
                {
                    var addFoodCategory = await _foodcategoryServices.AddFoodCategory(_mapper.Map<Foodcategory>(ModifyFoodCategory));
                    if (addFoodCategory != null)
                    {
                        ListFoodCategory.Add(_mapper.Map<FoodCategoryDTO>(addFoodCategory));
                        IsOpenModifyFoodCategory = false;
                        IsLoading = false;
                        MyMessageBox.ShowDialog("Thêm danh mục thực đơn thành công", MyMessageBox.Buttons.OK, MyMessageBox.Icons.Information);
                    }
                    else
                    {
                        MyMessageBox.ShowDialog("Thêm danh mục thực đơn cấp thất bại", MyMessageBox.Buttons.OK, MyMessageBox.Icons.Error);
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
                            IsOpenModifyFoodCategory = false;
                            IsLoading = false;
                            MyMessageBox.ShowDialog("Sửa danh mục thực đơn thành công", MyMessageBox.Buttons.OK, MyMessageBox.Icons.Information);
                        }
                    }
                    else
                    {
                        MyMessageBox.ShowDialog("Sửa danh mục thực đơn thất bại", MyMessageBox.Buttons.OK, MyMessageBox.Icons.Error);
                    }
                }
                CloseModifyFoodCategory();
            }
            catch (InvalidOperationException ioe)
            {
                MyMessageBox.ShowDialog(ioe.Message);
            }
            finally
            {
                IsLoading = false;
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
                    IsLoading = true;
                    bool isDeleted = await _foodcategoryServices.DeleteFoodCategory(foodCategory.Foodcategoryid);
                    if (isDeleted)
                    {
                        ListDeletedFoodCategory.Add(foodCategory);
                        ListFoodCategory.Remove(foodCategory);
                        IsLoading = false;
                        MyMessageBox.ShowDialog("Ẩn danh mục thanh công", MyMessageBox.Buttons.OK, MyMessageBox.Icons.Information);
                    }
                    else
                    {
                        MyMessageBox.ShowDialog("Ẩn danh mục thất bại", MyMessageBox.Buttons.OK, MyMessageBox.Icons.Error);
                    }
                }
            }
            catch (InvalidOperationException ioe)
            {
                MyMessageBox.ShowDialog(ioe.Message);
            }
            finally
            {
                IsLoading = false;
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
                    IsLoading = true;
                    foodCategory.Isdeleted = false;
                    var res = await _foodcategoryServices.UpdateFoodCategory(_mapper.Map<Foodcategory>(foodCategory));
                    if (res != null)
                    {
                        ListFoodCategory.Add(_mapper.Map<FoodCategoryDTO>(res));
                        ListDeletedFoodCategory.Remove(foodCategory);
                        IsLoading = false;
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
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        private void CloseModifyFoodCategory()
        {
            IsOpenModifyFoodCategory = false;
            IsAdding = false;
            IsUpdating = false;
            ModifyFoodCategory = new();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
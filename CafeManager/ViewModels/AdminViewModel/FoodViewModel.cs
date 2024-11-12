using CafeManager.Core.Data;
using CafeManager.Core.DTOs;
using CafeManager.WPF.Services;
using CafeManager.WPF.ViewModels.AddViewModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CafeManager.WPF.MessageBox;

namespace CafeManager.WPF.ViewModels.AdminViewModel
{
    public partial class FoodViewModel : ObservableObject, IDisposable
    {
        private readonly IServiceProvider _provider;
        private readonly FoodCategoryServices _foodCategoryServices;
        private readonly FoodServices _foodServices;
        private readonly FileDialogService _fileDialogService;

        [ObservableProperty]
        private ObservableCollection<Foodcategory> _listFoodCategory = [];

        [ObservableProperty]
        private ObservableCollection<FoodDTO> _listFoodByFoodCategoryId = [];

        [ObservableProperty]
        private Foodcategory _selectedFoodCategory;

        [ObservableProperty]
        private bool _isOpenAddFoodVM;

        [ObservableProperty]
        private AddUpdateFoodViewModel _addUpdateFoodVM;

        public FoodViewModel(IServiceProvider provider)
        {
            _provider = provider;
            _foodCategoryServices = provider.GetRequiredService<FoodCategoryServices>();
            _foodServices = provider.GetRequiredService<FoodServices>();
            _fileDialogService = provider.GetRequiredService<FileDialogService>();

            AddUpdateFoodVM = provider.GetRequiredService<AddUpdateFoodViewModel>();
            AddUpdateFoodVM.CloseVM += AddUpdateFoodVM_CloseVM;
            AddUpdateFoodVM.AddFoodChanged += AddUpdateFoodVM_AddFoodChanged;
            AddUpdateFoodVM.UpdateFoodChanged += AddUpdateFoodVM_UpdateFoodChangedAsync;

            _ = LoadData();
        }

        private async Task LoadData()
        {
            try
            {
                ListFoodCategory.Add(new Foodcategory()
                {
                    Foodcategoryname = "Tất cả"
                });

                IEnumerable<Foodcategory> listFoodCategory = await _foodCategoryServices.GetListFoodCategory();
                IEnumerable<Food> listFood = await _foodServices.GetAllListFood();

                AddUpdateFoodVM.ReceiveListFoodCategory(listFoodCategory.ToList());
                foreach (var item in listFoodCategory)
                {
                    ListFoodCategory.Add(item);
                }

                ListFoodByFoodCategoryId = new(listFood.Select(x => new FoodDTO()
                {
                    Id = x.Foodid,
                    Foodname = x.Foodname,
                    Price = x.Price,
                    Imagefood = _fileDialogService.Base64ToBitmapImage(x.Imagefood),
                    Discountfood = x.Discountfood,
                    Foodcategory = x.Foodcategory,
                }));
                SelectedFoodCategory = ListFoodCategory[0];
            }
            catch (Exception)
            {
            }
        }

        private async void AddUpdateFoodVM_UpdateFoodChangedAsync(Food obj)
        {
            try
            {
                Food updateFood = await _foodServices.GetFoodById(obj.Foodid);
                if (updateFood != null)
                {
                    updateFood.Price = obj.Price ?? decimal.Zero;
                    updateFood.Foodname = obj.Foodname;
                    updateFood.Discountfood = obj.Discountfood;
                    updateFood.Imagefood = obj.Imagefood;
                    updateFood.Foodcategoryid = obj.Foodcategoryid;

                    Food res = _foodServices.UpdatFood(updateFood);
                    if (res != null)
                    {
                        FoodDTO updateFoodDTO = ListFoodByFoodCategoryId.FirstOrDefault(x => x.Id == res.Foodid);

                        if (updateFoodDTO != null && updateFoodDTO.Foodcategory?.Foodcategoryid == res.Foodcategoryid || SelectedFoodCategory.Foodcategoryname.Equals("Tất cả"))
                        {
                            updateFoodDTO.Price = res.Price ?? decimal.Zero;
                            updateFoodDTO.Foodname = res.Foodname;
                            updateFoodDTO.Discountfood = res.Discountfood;
                            updateFoodDTO.Imagefood = _fileDialogService.Base64ToBitmapImage(res.Imagefood);
                            updateFoodDTO.Foodcategory = res.Foodcategory;
                            ListFoodByFoodCategoryId = new(ListFoodByFoodCategoryId);
                        }
                        else
                        {
                            ListFoodByFoodCategoryId.Remove(updateFoodDTO);
                        }

                        MyMessageBox.Show("Sửa thành công", MyMessageBox.Buttons.OK, MyMessageBox.Icons.Information);
                    }
                    else
                    {
                        MyMessageBox.Show("Lỗi", MyMessageBox.Buttons.OK, MyMessageBox.Icons.Error);
                    }
                    IsOpenAddFoodVM = false;
                    AddUpdateFoodVM.IsUpdating = false;
                    AddUpdateFoodVM.ClearValueOfForm();
                }
            }
            catch (InvalidOperationException ioe)
            {
                MyMessageBox.Show(ioe.Message, MyMessageBox.Buttons.OK, MyMessageBox.Icons.Warning);
            }
        }

        private async void AddUpdateFoodVM_AddFoodChanged(Food obj)
        {
            try
            {
                Food res = await _foodServices.AddFood(obj);
                if (res == null)
                {
                    MyMessageBox.Show("Không thêm được", MyMessageBox.Buttons.OK, MyMessageBox.Icons.Error);
                    return;
                }
                if (res.Foodcategoryid == SelectedFoodCategory.Foodcategoryid || SelectedFoodCategory.Foodcategoryname.Equals("Tất cả"))
                {
                    ListFoodByFoodCategoryId.Add(new FoodDTO()
                    {
                        Id = obj.Foodid,
                        Foodname = obj.Foodname,
                        Price = obj.Price ?? decimal.Zero,
                        Imagefood = _fileDialogService.Base64ToBitmapImage(obj.Imagefood),
                        Discountfood = obj.Discountfood,
                        Foodcategory = obj.Foodcategory,
                    });
                }
                MyMessageBox.Show("Thêm thành công", MyMessageBox.Buttons.OK, MyMessageBox.Icons.Information);
                AddUpdateFoodVM.ClearValueOfForm();
                IsOpenAddFoodVM = false;
                AddUpdateFoodVM.IsAdding = false;
            }
            catch (InvalidOperationException ivd)
            {
                MyMessageBox.Show(ivd.Message, MyMessageBox.Buttons.OK, MyMessageBox.Icons.Information);
            }
        }

        private void AddUpdateFoodVM_CloseVM()
        {
            IsOpenAddFoodVM = false;
        }

        [RelayCommand]
        private async Task SelectedFoodCategoryChange(Foodcategory foodcategory)
        {
            if (foodcategory == null) return;
            if (foodcategory.Foodcategoryname.Equals("Tất cả"))
            {
                var listAllFood = await _foodServices.GetAllListFood();

                ListFoodByFoodCategoryId = new(listAllFood.Select(x => new FoodDTO()
                {
                    Id = x.Foodid,
                    Foodname = x.Foodname,
                    Price = x.Price,
                    Imagefood = _fileDialogService.Base64ToBitmapImage(x.Imagefood),
                    Foodcategory = x.Foodcategory,
                    Discountfood = x.Discountfood,
                }));
            }
            else
            {
                var listFoodById = await _foodCategoryServices.GetListFoodByFoodCatgoryId(foodcategory.Foodcategoryid);

                ListFoodByFoodCategoryId = new(listFoodById.Select(x => new FoodDTO()
                {
                    Id = x.Foodid,
                    Foodname = x.Foodname,
                    Price = x.Price,
                    Imagefood = _fileDialogService.Base64ToBitmapImage(x.Imagefood),
                    Discountfood = x.Discountfood,
                    Foodcategory = x.Foodcategory
                }));
            }
        }

        [RelayCommand]
        private void OpenAddFood()
        {
            AddUpdateFoodVM.IsAdding = true;
            IsOpenAddFoodVM = true;
        }

        [RelayCommand]
        private void OpenUpdateFood(FoodDTO foodDTO)
        {
            AddUpdateFoodVM.IsUpdating = true;
            AddUpdateFoodVM.ReceiveFood(foodDTO);
            IsOpenAddFoodVM = true;
        }

        [RelayCommand]
        private async Task DeleteFood(FoodDTO foodDTO)
        {
            try
            {
                bool isdeleted = await _foodServices.DeletFood(foodDTO.Id);
                if (isdeleted)
                {
                    MyMessageBox.Show("Xóa thành công thức ăn", MyMessageBox.Buttons.OK, MyMessageBox.Icons.Information);
                }

                ListFoodByFoodCategoryId.Remove(foodDTO);
            }
            catch (InvalidOperationException ivd)
            {
                MyMessageBox.Show(ivd.Message, MyMessageBox.Buttons.OK, MyMessageBox.Icons.Warning);
                throw;
            }
        }

        public void Dispose()
        {
            if (AddUpdateFoodVM != null)
            {
                AddUpdateFoodVM.CloseVM -= AddUpdateFoodVM_CloseVM;
                AddUpdateFoodVM.AddFoodChanged -= AddUpdateFoodVM_AddFoodChanged;
                AddUpdateFoodVM.UpdateFoodChanged -= AddUpdateFoodVM_UpdateFoodChangedAsync;
            }
            GC.SuppressFinalize(this);
        }
    }
}
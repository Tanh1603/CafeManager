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

namespace CafeManager.WPF.ViewModels.AdminViewModel
{
    public partial class FoodViewModel : ObservableObject, IDisposable
    {
        private readonly IServiceProvider _provider;
        private readonly FoodCategoryServices _foodCategoryServices;
        private readonly FoodServices _foodServices;
        private readonly FileDialogService _fileDialogService;

        [ObservableProperty]
        private ObservableCollection<FoodCategoryDTO> _listFoodCategory = [];

        [ObservableProperty]
        private ObservableCollection<FoodDTO> _listFoodByFoodCategoryId = [];

        [ObservableProperty]
        private FoodCategoryDTO _selectedFoodCategory;

        [ObservableProperty]
        private bool _isOpenModifyFoodView;

        [ObservableProperty]
        private ModifyFoodViewModel _modifyFoodVM;

        [ObservableProperty]
        private bool _isOpenModifyFCView;

        public FoodViewModel(IServiceProvider provider)
        {
            _provider = provider;
            _foodCategoryServices = provider.GetRequiredService<FoodCategoryServices>();
            _foodServices = provider.GetRequiredService<FoodServices>();
            _fileDialogService = provider.GetRequiredService<FileDialogService>();
            ModifyFoodVM = provider.GetRequiredService<ModifyFoodViewModel>();
            ModifyFoodVM.ModifyFoodChanged += ModifyFoodVM_ModifyFoodChanged;
            Task.Run(LoadData);
        }

        private async Task LoadData()
        {
            var dbListFoodCategory = await _foodCategoryServices.GetAllListFoodCategory();
            ListFoodCategory = [.. dbListFoodCategory.Select(fc => FoodCategoryMapper.ToDTO(fc))];

            SelectedFoodCategory = ListFoodCategory[0];
            var dbListFoodByFoodCategoryId =
                await _foodServices.GetAllListFoodByFoodCategoryId(SelectedFoodCategory.Foodcategoryid);
            ListFoodByFoodCategoryId = [.. dbListFoodByFoodCategoryId.Select(f => FoodMapper.ToDTO(f))];
            ModifyFoodVM.ReceiveListFoodCategory(ListFoodCategory.ToList());
        }

        private async void ModifyFoodVM_ModifyFoodChanged(FoodDTO foodDTO)
        {
            try
            {
                if (ModifyFoodVM.IsAdding)
                {
                    Food addFood = await _foodServices.CreateFood(FoodMapper.ToEntity(foodDTO));
                    if (addFood != null)
                    {
                        FoodDTO addFoodDTO = FoodMapper.ToDTO(addFood);
                        if (addFoodDTO.Foodcategoryid == SelectedFoodCategory.Foodcategoryid)
                        {
                            ListFoodByFoodCategoryId.Add(addFoodDTO);
                        }
                        MyMessageBox.Show("Thêm thức ăn thành công");
                    }
                }
                if (ModifyFoodVM.IsUpdating)
                {
                    var updateFood = await _foodServices.GetFoodById(foodDTO.Foodid);
                    Food updateFoodEntity = FoodMapper.ToEntity(foodDTO);

                    updateFood.Foodname = updateFoodEntity.Foodname;
                    updateFood.Foodcategoryid = updateFoodEntity.Foodcategoryid;
                    updateFood.Price = updateFoodEntity.Price;
                    updateFood.Imagefood = updateFoodEntity.Imagefood;
                    updateFood.Discountfood = updateFoodEntity.Discountfood;
                    var res = _foodServices.UpdatFood(updateFood);

                    if (res != null)
                    {
                        var updateFoodDTO = ListFoodByFoodCategoryId
                                            .FirstOrDefault(f => f.Foodid == res.Foodid);
                        if (updateFoodDTO != null)
                        {
                            FoodDTO foodMapper = FoodMapper.ToDTO(res);
                            if (foodMapper.Foodcategoryid == SelectedFoodCategory.Foodcategoryid)
                            {
                                updateFoodDTO.Foodid = foodMapper.Foodid;
                                updateFoodDTO.Foodname = foodMapper.Foodname;
                                updateFoodDTO.Foodcategoryid = foodMapper.Foodcategoryid;
                                updateFoodDTO.Price = foodMapper.Price;
                                updateFoodDTO.Discountfood = foodMapper.Discountfood;
                                updateFoodDTO.Imagefood = foodMapper.Imagefood;
                            }
                            else
                            {
                                ListFoodByFoodCategoryId.Remove(updateFoodDTO);
                            }
                            MyMessageBox.Show("Sửa thức ăn thành công");
                        }
                    }
                }
                IsOpenModifyFoodView = false;
                ModifyFoodVM.ClearValueOfForm();
            }
            catch (InvalidOperationException ioe)
            {
                MyMessageBox.Show(ioe.Message);
            }
        }

        [RelayCommand]
        private async Task SelectedFoodCategoryChange(FoodCategoryDTO foodcategory)
        {
            if (foodcategory == null) return;
            var dbListFoodByFoodCategoryId = await _foodServices.GetAllListFoodByFoodCategoryId(foodcategory.Foodcategoryid);
            ListFoodByFoodCategoryId = [.. dbListFoodByFoodCategoryId.Select(f => FoodMapper.ToDTO(f)).ToList()];
        }

        [RelayCommand]
        private void CloseModifyFoodView()
        {
            IsOpenModifyFoodView = false;
            ModifyFoodVM.ClearValueOfForm();
        }

        [RelayCommand]
        private void OpenAddFood()
        {
            ModifyFoodVM.IsAdding = true;
            IsOpenModifyFoodView = true;
        }

        [RelayCommand]
        private void OpenUpdateFood(FoodDTO foodDTO)
        {
            ModifyFoodVM.IsUpdating = true;
            ModifyFoodVM.ReceiveFood(foodDTO);
            IsOpenModifyFoodView = true;
        }

        [RelayCommand]
        private async Task DeleteFood(FoodDTO foodDTO)
        {
            try
            {
                string res = MyMessageBox.ShowDialog("Bạn muốn ẩn món ăn này trên menu không", MyMessageBox.Buttons.Yes_No, MyMessageBox.Icons.Warning);
                if (res.Equals("1"))
                {
                    bool isdeleted = await _foodServices.DeletFood(foodDTO.Foodid);
                    if (isdeleted)
                    {
                        MyMessageBox.Show("Ẩn món ăn thành công");
                        var deletedFoodDTO = ListFoodByFoodCategoryId.FirstOrDefault(x => x.Foodid == foodDTO.Foodid);
                        if (deletedFoodDTO != null)
                        {
                            deletedFoodDTO.Isdeleted = true;
                        }
                    }
                }
            }
            catch (InvalidOperationException ivd)
            {
                MyMessageBox.Show(ivd.Message, MyMessageBox.Buttons.OK, MyMessageBox.Icons.Error);
            }
        }

        [RelayCommand]
        private async Task RestoreFood(FoodDTO foodDTO)
        {
            try
            {
                string res = MyMessageBox.ShowDialog("Bạn muốn hiện món ăn này trên menu không", MyMessageBox.Buttons.Yes_No, MyMessageBox.Icons.Warning);
                if (res.Equals("1"))
                {
                    var showFood = await _foodServices.GetDeletedFoodById(foodDTO.Foodid);
                    if (showFood != null)
                    {
                        showFood.Isdeleted = false;
                        _foodServices.UpdatFood(showFood);

                        var showFoodDTO = ListFoodByFoodCategoryId.FirstOrDefault(x => x.Foodid == foodDTO.Foodid);
                        if (showFoodDTO != null)
                        {
                            showFoodDTO.Isdeleted = false;
                        }
                        MyMessageBox.Show("Hiện món ăn thành công");
                    }
                }
            }
            catch (InvalidOperationException ivd)
            {
                MyMessageBox.Show(ivd.Message, MyMessageBox.Buttons.OK, MyMessageBox.Icons.Error);
            }
        }

        [RelayCommand]
        private void OpenPopupFoodCategoryView()
        {
            IsOpenModifyFCView = true;
        }

        public void Dispose()
        {
            ModifyFoodVM.ModifyFoodChanged -= ModifyFoodVM_ModifyFoodChanged;
            GC.SuppressFinalize(this);
        }
    }
}
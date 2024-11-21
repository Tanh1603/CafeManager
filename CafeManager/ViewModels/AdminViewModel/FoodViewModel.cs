using AutoMapper;
using CafeManager.Core.Data;
using CafeManager.Core.DTOs;
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
        private readonly IMapper _mapper;

        [ObservableProperty]
        private ObservableCollection<FoodCategoryDTO> _listFoodCategory = [];

        [ObservableProperty]
        private ObservableCollection<FoodDTO> _listFoodByFoodCategoryId = [];

        private FoodCategoryDTO _selectedFoodCategory = new();

        public FoodCategoryDTO SelectedFoodCategory
        {
            get => _selectedFoodCategory; set
            {
                if (_selectedFoodCategory != value)
                {
                    if (value != null)
                    {
                        ListFoodByFoodCategoryId = ListFoodCategory.FirstOrDefault(x => x.Foodcategoryid == value.Foodcategoryid)?.Foods ?? [];
                        _selectedFoodCategory = value;
                    }
                }
                OnPropertyChanged();
            }
        }

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
            _mapper = provider.GetRequiredService<IMapper>();
            ModifyFoodVM = provider.GetRequiredService<ModifyFoodViewModel>();
            ModifyFoodVM.ModifyFoodChanged += ModifyFoodVM_ModifyFoodChanged;
            Task.Run(LoadData);
        }

        private async Task LoadData()
        {
            var dbListFoodCategory = await _foodCategoryServices.GetAllListFoodCategory();
            ListFoodCategory = [.. _mapper.Map<List<FoodCategoryDTO>>(dbListFoodCategory)];
            SelectedFoodCategory = ListFoodCategory[0];
            ListFoodByFoodCategoryId = ListFoodCategory.FirstOrDefault(x => x.Foodcategoryid == SelectedFoodCategory.Foodcategoryid)?.Foods ?? [];
            ModifyFoodVM.ReceiveListFoodCategory(ListFoodCategory.ToList());
        }

        private async void ModifyFoodVM_ModifyFoodChanged(FoodDTO foodDTO)
        {
            //try
            //{
            //    if (ModifyFoodVM.IsAdding)
            //    {
            //        Food addFood = await _foodServices.CreateFood(FoodMapper.ToEntity(foodDTO));
            //        if (addFood != null)
            //        {
            //            FoodDTO addFoodDTO = FoodMapper.ToDTO(addFood);
            //            if (addFoodDTO.Foodcategoryid == SelectedFoodCategory.Foodcategoryid)
            //            {
            //                ListFoodByFoodCategoryId.Add(addFoodDTO);
            //            }
            //            MyMessageBox.Show("Thêm thức ăn thành công");
            //        }
            //    }
            //    if (ModifyFoodVM.IsUpdating)
            //    {
            //        var updateFood = await _foodServices.GetFoodById(foodDTO.Foodid);
            //        Food updateFoodEntity = FoodMapper.ToEntity(foodDTO);

            //        updateFood.Foodname = updateFoodEntity.Foodname;
            //        updateFood.Foodcategoryid = updateFoodEntity.Foodcategoryid;
            //        updateFood.Price = updateFoodEntity.Price;
            //        updateFood.Imagefood = updateFoodEntity.Imagefood;
            //        updateFood.Discountfood = updateFoodEntity.Discountfood;
            //        var res = _foodServices.UpdatFood(updateFood);

            //        if (res != null)
            //        {
            //            var updateFoodDTO = ListFoodByFoodCategoryId
            //                                .FirstOrDefault(f => f.Foodid == res.Foodid);
            //            if (updateFoodDTO != null)
            //            {
            //                FoodDTO foodMapper = FoodMapper.ToDTO(res);
            //                if (foodMapper.Foodcategoryid == SelectedFoodCategory.Foodcategoryid)
            //                {
            //                    updateFoodDTO.Foodid = foodMapper.Foodid;
            //                    updateFoodDTO.Foodname = foodMapper.Foodname;
            //                    updateFoodDTO.Foodcategoryid = foodMapper.Foodcategoryid;
            //                    updateFoodDTO.Price = foodMapper.Price;
            //                    updateFoodDTO.Discountfood = foodMapper.Discountfood;
            //                    updateFoodDTO.Imagefood = foodMapper.Imagefood;
            //                }
            //                else
            //                {
            //                    ListFoodByFoodCategoryId.Remove(updateFoodDTO);
            //                }
            //                MyMessageBox.Show("Sửa thức ăn thành công");
            //            }
            //        }
            //    }
            //    IsOpenModifyFoodView = false;
            //    ModifyFoodVM.ClearValueOfForm();
            //}
            //catch (InvalidOperationException ioe)
            //{
            //    MyMessageBox.Show(ioe.Message);
            //}
            try
            {
                if (ModifyFoodVM.IsAdding)
                {
                    Food addFood = await _foodServices.CreateFood(_mapper.Map<Food>(foodDTO));
                    if (addFood != null)
                    {
                        FoodDTO addFoodDTO = _mapper.Map<FoodDTO>(addFood);
                        if (addFoodDTO.Foodcategoryid == SelectedFoodCategory.Foodcategoryid)
                        {
                            ListFoodByFoodCategoryId.Add(addFoodDTO);
                        }
                        else
                        {
                            ListFoodCategory.FirstOrDefault(x => x.Foodcategoryid == addFoodDTO.Foodcategoryid)?.Foods.Add(addFoodDTO);
                        }
                        MyMessageBox.Show("Thêm thức ăn thành công");
                    }
                }
                if (ModifyFoodVM.IsUpdating)
                {
                    Food? updateFood = await _foodServices.GetFoodById(foodDTO.Foodid);
                    if (updateFood != null)
                    {
                        Food updateFoodEntity = _mapper.Map<Food>(foodDTO);
                        updateFood.Foodname = updateFoodEntity.Foodname;
                        updateFood.Foodcategoryid = updateFoodEntity.Foodcategoryid;
                        updateFood.Price = updateFoodEntity.Price;
                        updateFood.Imagefood = updateFoodEntity.Imagefood;
                        updateFood.Discountfood = updateFoodEntity.Discountfood;
                        Food? res = _foodServices.UpdatFood(updateFood);
                        if (res != null)
                        {
                            var updateFoodDTO = ListFoodByFoodCategoryId
                                                .FirstOrDefault(f => f.Foodid == res.Foodid);
                            if (updateFoodDTO != null)
                            {
                                FoodDTO foodMapper = _mapper.Map<FoodDTO>(res);
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
                    var showFood = await _foodServices.GetFoodById(foodDTO.Foodid);
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
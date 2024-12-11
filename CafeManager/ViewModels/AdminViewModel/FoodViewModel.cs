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
using System.Diagnostics;

namespace CafeManager.WPF.ViewModels.AdminViewModel
{
    public partial class FoodViewModel : ObservableObject, IDisposable, IDataViewModel
    {
        private readonly FoodCategoryServices _foodCategoryServices;
        private readonly FoodServices _foodServices;
        private readonly IMapper _mapper;
        private CancellationToken _token = default;

        [ObservableProperty]
        private ObservableCollection<FoodCategoryDTO> _listFoodCategory = [];

        public ObservableCollection<FoodDTO> ListFoodByFoodCategoryId => [.. _filterListFood ?? []];
        private List<FoodDTO> _filterListFood = [];
        private FoodCategoryDTO _selectedFoodCategory = new();

        public FoodCategoryDTO SelectedFoodCategory
        {
            get => _selectedFoodCategory; set
            {
                if (_selectedFoodCategory != value)
                {
                    if (value != null)
                    {
                        _selectedFoodCategory = value;
                        FilterListFood();
                        OnPropertyChanged(nameof(ListFoodByFoodCategoryId));
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

        private string _searchText = string.Empty;

        public string SearchText
        {
            get => _searchText;
            set
            {
                if (_searchText != value)
                {
                    _searchText = value;
                    FilterListFood();
                    OnPropertyChanged(nameof(SearchText));
                }
            }
        }

        public FoodViewModel(IServiceScope scope)
        {
            var provider = scope.ServiceProvider;
            _foodCategoryServices = provider.GetRequiredService<FoodCategoryServices>();
            _foodServices = provider.GetRequiredService<FoodServices>();
            _mapper = provider.GetRequiredService<IMapper>();
            ModifyFoodVM = provider.GetRequiredService<ModifyFoodViewModel>();
            ModifyFoodVM.ModifyFoodChanged += ModifyFoodVM_ModifyFoodChanged;
        }

        public async Task LoadData(CancellationToken token = default)
        {
            try
            {
                if (_token == default)
                {
                    _token = token;
                }
                token.ThrowIfCancellationRequested();
                var dbListFoodCategory = await _foodCategoryServices.GetAllListFoodCategory(token);
                ListFoodCategory = [.. _mapper.Map<List<FoodCategoryDTO>>(dbListFoodCategory)];
                SelectedFoodCategory = ListFoodCategory[0];
                FilterListFood();
                ModifyFoodVM.ReceiveListFoodCategory(ListFoodCategory.ToList());
            }
            catch (OperationCanceledException)
            {
                Debug.WriteLine("LoadData của FoodViewModel bị hủy");
            }
        }

        private void FilterListFood()
        {
            var filter = SelectedFoodCategory.Foods?.Where(x => string.IsNullOrWhiteSpace(SearchText)
            || x.Foodname.Contains(SearchText, StringComparison.OrdinalIgnoreCase) || x.Price.ToString().Contains(SearchText)).ToList();

            _filterListFood = [.. filter];
            OnPropertyChanged(nameof(ListFoodByFoodCategoryId));
        }

        private async void ModifyFoodVM_ModifyFoodChanged(FoodDTO foodDTO)
        {
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
                            SelectedFoodCategory.Foods.Add(addFoodDTO);
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
                    var res = await _foodServices.UpdatFood(_mapper.Map<Food>(foodDTO));
                    if (res != null)
                    {
                        var updateFoodDTO = SelectedFoodCategory.Foods.FirstOrDefault(x => x.Foodid == res.Foodid);
                        if (updateFoodDTO != null)
                        {
                            if (updateFoodDTO.Foodcategoryid == res.Foodcategoryid)
                            {
                                _mapper.Map(res, updateFoodDTO);
                            }
                            else
                            {
                                SelectedFoodCategory.Foods.Remove(updateFoodDTO);
                                ListFoodCategory.FirstOrDefault(x => x.Foodcategoryid == res.Foodcategoryid)?.Foods.Add(_mapper.Map<FoodDTO>(res));
                            }
                            MyMessageBox.Show("Sửa thức ăn thành công");
                        }
                    }
                }
                IsOpenModifyFoodView = false;
                ModifyFoodVM.ClearValueOfForm();
                FilterListFood();
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
                        var deletedFoodDTO = SelectedFoodCategory.Foods.FirstOrDefault(x => x.Foodid == foodDTO.Foodid);
                        if (deletedFoodDTO != null)
                        {
                            deletedFoodDTO.Isdeleted = true;
                        }
                        MyMessageBox.Show("Ẩn món ăn thành công");
                        OnPropertyChanged(nameof(ListFoodByFoodCategoryId));
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
                        await _foodServices.UpdatFood(showFood);

                        var showFoodDTO = SelectedFoodCategory.Foods.FirstOrDefault(x => x.Foodid == foodDTO.Foodid);
                        if (showFoodDTO != null)
                        {
                            showFoodDTO.Isdeleted = false;
                        }
                        MyMessageBox.Show("Hiện món ăn thành công");
                        OnPropertyChanged(nameof(ListFoodByFoodCategoryId));
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
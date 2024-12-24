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
using Newtonsoft.Json.Linq;
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
        private List<FoodDTO> _allFood = [];

        [ObservableProperty]
        private bool _isLoading;

        [ObservableProperty]
        private ObservableCollection<FoodCategoryDTO> _listFoodCategory = [];

        public ObservableCollection<FoodDTO> ListFoodByFoodCategoryId => [.. _filterListFood ?? []];
        private List<FoodDTO> _filterListFood = [];
        private FoodCategoryDTO? _selectedFoodCategory;

        public FoodCategoryDTO? SelectedFoodCategory
        {
            get => _selectedFoodCategory; set
            {
                if (_selectedFoodCategory != value)
                {
                    _selectedFoodCategory = value;
                    FilterListFood();
                }
                OnPropertyChanged();
            }
        }

        [ObservableProperty]
        private bool _isOpenModifyFoodView;

        [ObservableProperty]
        private ModifyFoodViewModel _modifyFoodVM;

        [ObservableProperty]
        private FoodCategoryViewModel _foodCategoryVM;

        [ObservableProperty]
        private bool _isOpenFoodCategoryView;

        private string _searchText = string.Empty;

        public string SearchText
        {
            get => _searchText;
            set
            {
                if (_searchText != value)
                {
                    _searchText = value;
                    OnPropertyChanged(nameof(SearchText));
                    FilterListFood();
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
            FoodCategoryVM = provider.GetRequiredService<FoodCategoryViewModel>();
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
                IsLoading = true;
                var dbListFoodCategory = await _foodCategoryServices.GetAllListFoodCategory(token);
                var dbListAllFood = await _foodServices.GetAllFood(token);
                var listExist = dbListFoodCategory.Where(x => x.Isdeleted == false).ToList();
                var listDeleted = dbListFoodCategory.Where(x => x.Isdeleted == true).ToList();

                FoodCategoryVM.ListFoodCategory = [.. _mapper.Map<List<FoodCategoryDTO>>(listExist)];
                FoodCategoryVM.ListDeletedFoodCategory = [.. _mapper.Map<List<FoodCategoryDTO>>(listDeleted)];
                ListFoodCategory = [.. _mapper.Map<List<FoodCategoryDTO>>(listExist)];
                _allFood = [.. _mapper.Map<List<FoodDTO>>(dbListAllFood).Where(x => x.Foodcategory.Isdeleted == false)];
                ModifyFoodVM.ReceiveListFoodCategory([.. ListFoodCategory]);
                FilterListFood();
                IsLoading = false;
            }
            catch (OperationCanceledException)
            {
                Debug.WriteLine("LoadData của FoodViewModel bị hủy");
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void FilterListFood()
        {
            _filterListFood = _allFood.Where(x => (string.IsNullOrWhiteSpace(SearchText) || x.Foodname.Contains(SearchText, StringComparison.OrdinalIgnoreCase) || x.Price.ToString().Contains(SearchText)) &&
            (SelectedFoodCategory == null || x.Foodcategoryid == SelectedFoodCategory.Foodcategoryid)).ToList();
            OnPropertyChanged(nameof(ListFoodByFoodCategoryId));
        }

        private async void ModifyFoodVM_ModifyFoodChanged(FoodDTO foodDTO)
        {
            try
            {
                IsLoading = true;
                if (ModifyFoodVM.IsAdding)
                {
                    Food addFood = await _foodServices.CreateFood(_mapper.Map<Food>(foodDTO));
                    if (addFood != null)
                    {
                        FoodDTO addFoodDTO = _mapper.Map<FoodDTO>(addFood);
                        _allFood.Add(addFoodDTO);
                        IsLoading = false;
                        MyMessageBox.Show("Thêm thức ăn thành công");
                    }
                }
                if (ModifyFoodVM.IsUpdating)
                {
                    var res = await _foodServices.UpdatFood(_mapper.Map<Food>(foodDTO));
                    if (res != null)
                    {
                        _mapper.Map(res, _allFood.Find(x => x.Foodid == res.Foodid));
                        IsLoading = false;
                        MyMessageBox.Show("Sửa thức ăn thành công");
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
            finally
            {
                IsLoading = false;
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
                    IsLoading = true;
                    bool isdeleted = await _foodServices.DeletFood(foodDTO.Foodid);
                    if (isdeleted)
                    {
                        var deletedFoodDTO = _allFood.FirstOrDefault(x => x.Foodid == foodDTO.Foodid);
                        if (deletedFoodDTO != null)
                        {
                            deletedFoodDTO.Isdeleted = true;
                        }
                        IsLoading = false;
                        MyMessageBox.Show("Ẩn món ăn thành công");
                        FilterListFood();
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
                    IsLoading = true;
                    var showFood = _allFood.Find(x => x.Foodid == foodDTO.Foodid);
                    if (showFood != null)
                    {
                        showFood.Isdeleted = false;
                        await _foodServices.UpdatFood(_mapper.Map<Food>(showFood));
                        IsLoading = false;
                        MyMessageBox.Show("Hiện món ăn thành công");
                        FilterListFood();
                    }
                }
            }
            catch (InvalidOperationException ivd)
            {
                MyMessageBox.Show(ivd.Message, MyMessageBox.Buttons.OK, MyMessageBox.Icons.Error);
            }
        }

        [RelayCommand]
        private void OpenFoodCategoryView()
        {
            IsOpenFoodCategoryView = true;
        }

        [RelayCommand]
        private async Task CloseFoodCategoryView()
        {
            try
            {
                _token.ThrowIfCancellationRequested();
                IsOpenFoodCategoryView = false;
                ListFoodCategory = FoodCategoryVM.ListFoodCategory;
                IsLoading = true;
                var dbListAllFood = await _foodServices.GetAllFood(_token);
                _allFood = [.. _mapper.Map<List<FoodDTO>>(dbListAllFood).Where(x => x.Foodcategory.Isdeleted == false)];
                FilterListFood();
                SelectedFoodCategory = null;
                IsLoading = false;
                OnPropertyChanged(nameof(ListFoodCategory));
                ModifyFoodVM.ReceiveListFoodCategory([.. ListFoodCategory]);
            }
            catch (OperationCanceledException)
            {
                throw;
            }
        }

        public void Dispose()
        {
            ModifyFoodVM.ModifyFoodChanged -= ModifyFoodVM_ModifyFoodChanged;
            GC.SuppressFinalize(this);
        }
    }
}
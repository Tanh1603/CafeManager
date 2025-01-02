using AutoMapper;
using CafeManager.Core.Data;
using CafeManager.Core.DTOs;
using CafeManager.Core.Services;
using CafeManager.WPF.MessageBox;
using CafeManager.WPF.Services;
using CafeManager.WPF.ViewModels.UserViewModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using CafeManager.WPF.Services.Converter;
using System.Windows;
using CafeManager.WPF.ViewModels.AddViewModel;

namespace CafeManager.WPF.ViewModels.UserViewModel
{
    public partial class DistributionMaterialViewModel : ObservableObject, IDataViewModel, IDisposable
    {
        private readonly MaterialSupplierServices _materialSupplierServices;
        private readonly ConsumedMaterialServices _consumedMaterialServices;
        private readonly IMapper _mapper;

        [ObservableProperty]
        private bool _isLoading;

        [ObservableProperty]
        private List<MaterialDTO> _listMaterialDTO = [];

        #region Inventory Delcare

        [ObservableProperty]
        private bool _isOpenReturnMaterial;

        [ObservableProperty]
        private ConsumedMaterialDTO _modifyConsumedMaterial = new();

        [ObservableProperty]
        private decimal _returnQuantity = 0;

        [ObservableProperty]
        private ObservableCollection<ConsumedMaterialDTO> _listConsumedMaterialDTO = [];

        [ObservableProperty]
        private bool _isOpenRequestMaterialView;

        [ObservableProperty]
        private RequestMaterialViewModel _requestMaterialVM;

        [ObservableProperty]
        private bool _isOpenSelectInventory;

        [ObservableProperty]
        private SelectInventoryViewModel _selectInventoryVM;

        #endregion Inventory Delcare

        // ===================== Filter Declare =====================

        #region Filter Declare

        private MaterialDTO? _selectedMaterial;

        public MaterialDTO? SelectedMaterial
        {
            get => _selectedMaterial;
            set
            {
                if (_selectedMaterial != value)
                {
                    _selectedMaterial = value;
                    OnPropertyChanged(nameof(SelectedMaterial));
                    _ = FirstPage();
                }
            }
        }

        private DateTime? _filterUseDate = DateTime.Today;

        public DateTime? FilterUseDate
        {
            get => _filterUseDate;
            set
            {
                if (_filterUseDate != value)
                {
                    _filterUseDate = value;
                    OnPropertyChanged(nameof(FilterUseDate));
                    _ = FirstPage();
                }
            }
        }

        private Expression<Func<Consumedmaterial, bool>> ConsumedMaterialFilter => consumedMaterial =>
            (consumedMaterial.Isdeleted == false) &&
            (SelectedMaterial == null || consumedMaterial.Materialsupplier.Materialid == SelectedMaterial.Materialid) &&
            (FilterUseDate == null ||
            (FilterUseDate.Value.Day == consumedMaterial.Usagedate.Day && FilterUseDate.Value.Month == consumedMaterial.Usagedate.Month && FilterUseDate.Value.Year == consumedMaterial.Usagedate.Year));

        #endregion Filter Declare

        public DistributionMaterialViewModel(IServiceScope scope)
        {
            var provider = scope.ServiceProvider;
            _materialSupplierServices = provider.GetRequiredService<MaterialSupplierServices>();
            _consumedMaterialServices = provider.GetRequiredService<ConsumedMaterialServices>();
            _mapper = provider.GetRequiredService<IMapper>();

            RequestMaterialVM = provider.GetRequiredService<RequestMaterialViewModel>();
            RequestMaterialVM.OpenSelectInventoryView += RequestMaterialVM_OpenSelectInventoryView;
            RequestMaterialVM.Submit += RequestMaterialVM_Submit;
            SelectInventoryVM = provider.GetRequiredService<SelectInventoryViewModel>();
            SelectInventoryVM.Commit += SelectInventoryVM_Commit;
            IsOpenRequestMaterialView = false;
        }

        public async Task LoadData(CancellationToken token = default)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                IsLoading = true;
                var dbMaterial = (await _materialSupplierServices.GetListMaterial(token)).Where(x => x.Isdeleted == false).ToList();
                var dbSupplier = (await _materialSupplierServices.GetListExistedSupplier()).ToList();
                var dbInventory = (await _materialSupplierServices.GetListMaterialSupplier(token)).Where(x => x.Isdeleted == false).ToList();

                ListMaterialDTO = [.. _mapper.Map<List<MaterialDTO>>(dbMaterial)];
                await LoadConsumedMaterial(token);
                SelectInventoryVM.ReceiveListMaterial(ListMaterialDTO);
                SelectInventoryVM.ReceiveListSupplier([.. _mapper.Map<List<SupplierDTO>>(dbSupplier)]);
                IsLoading = false;
            }
            catch (OperationCanceledException)
            {
                throw new OperationCanceledException("LoadData DistributionViewModel bị hủy");
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task LoadConsumedMaterial(CancellationToken token = default)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                IsLoading = true;
                var dblistConsumedMaterial = await _consumedMaterialServices.GetSearchPaginateListConsumedMaterial(ConsumedMaterialFilter, pageIndex, pageSize, token);
                ListConsumedMaterialDTO = [.. _mapper.Map<List<ConsumedMaterialDTO>>(dblistConsumedMaterial.Item1)];
                TotalPages = (dblistConsumedMaterial.Item2 + pageSize - 1) / pageSize;
                OnPropertyChanged(nameof(PageUI));
                IsLoading = false;
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                IsLoading = false;
            }
        }

        #region Open/Close View

        [RelayCommand]
        private void OpenRequestMaterialView()
        {
            IsOpenRequestMaterialView = true;
        }

        private void SelectInventoryVM_Commit()
        {
            RequestMaterialVM.AddConsumedMaterial(SelectInventoryVM.SelectedInventory);
            CloseSelectInventoryView();
        }

        [RelayCommand]
        private void CloseSelectInventoryView()
        {
            IsOpenSelectInventory = false;
            SelectInventoryVM.ClearValue();
        }

        private void RequestMaterialVM_OpenSelectInventoryView()
        {
            _ = SelectInventoryVM.LoadData();
            IsOpenSelectInventory = true;
        }

        private async void RequestMaterialVM_Submit(List<ConsumedMaterialDTO> consumedMaterialDTOs)
        {
            foreach (var addconsumed in consumedMaterialDTOs)
            {
                var consumed = ListConsumedMaterialDTO.FirstOrDefault(x => x.Materialsupplierid == addconsumed.Materialsupplierid && x.Usagedate == addconsumed.Usagedate);
                if (consumed != null)
                {
                    consumed.Quantity += addconsumed.Quantity;
                    var updateConsumed = await _consumedMaterialServices.UpdateConsumedmaterial(_mapper.Map<Consumedmaterial>(consumed));
                }
                else
                {
                    Consumedmaterial? addConsumedMaterial = await _consumedMaterialServices.AddConsumedMaterial(_mapper.Map<Consumedmaterial>(addconsumed));
                }
                MyMessageBox.ShowDialog("Thêm chi tiết sử dụng vật liệu thành công", MyMessageBox.Buttons.OK, MyMessageBox.Icons.Information);
                await LoadConsumedMaterial();
            }
            IsOpenRequestMaterialView = false;
            RequestMaterialVM.ClearValue();
        }

        [RelayCommand]
        private void CloseRequestMaterialView()
        {
            RequestMaterialVM.ClearValue();
            IsOpenRequestMaterialView = false;
        }

        [RelayCommand]
        private void ReturnMaterial(ConsumedMaterialDTO consumed)
        {
            IsOpenReturnMaterial = true;
            ModifyConsumedMaterial = consumed.Clone();
        }

        [RelayCommand]
        private void CloseReturnMaterial()
        {
            IsOpenReturnMaterial = false;
            ModifyConsumedMaterial = new();
            ReturnQuantity = 0;
        }

        [RelayCommand]
        private async Task SubmitReturnMaterial()
        {
            try
            {
                if (ReturnQuantity > ModifyConsumedMaterial.Quantity)
                    throw new InvalidOperationException("Số lượng vật liệu trả lại lớn hơn số lượng vật liệu đã lấy!");
                ModifyConsumedMaterial.Quantity -= ReturnQuantity;
                var res = await _consumedMaterialServices.UpdateConsumedmaterial(_mapper.Map<Consumedmaterial>(ModifyConsumedMaterial));
                if (res != null)
                {
                    MyMessageBox.ShowDialog("Hoàn trả vật liệu thành công", MyMessageBox.Buttons.OK, MyMessageBox.Icons.Information);
                    await LoadConsumedMaterial();
                }
                else
                {
                    MyMessageBox.ShowDialog("Hoàn trả vật liệu thất bại", MyMessageBox.Buttons.OK, MyMessageBox.Icons.Error);
                }
                CloseReturnMaterial();
            }
            catch (InvalidOperationException ioe)
            {
                MyMessageBox.ShowDialog(ioe.Message, MyMessageBox.Buttons.OK, MyMessageBox.Icons.Warning);
            }
        }

        #endregion Open/Close View

        public void Dispose()
        {
            RequestMaterialVM.OpenSelectInventoryView -= RequestMaterialVM_OpenSelectInventoryView;
            RequestMaterialVM.Submit -= RequestMaterialVM_Submit;
            SelectInventoryVM.Commit -= SelectInventoryVM_Commit;
            GC.SuppressFinalize(this);
        }

        #region Phan Trang ConsumedMaterial

        private int pageIndex = 1;

        private int pageSize = 8;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(PageVisibility))]
        private int totalPages = 0;

        public bool PageVisibility => TotalPages > 0;
        public string PageUI => $"{pageIndex}/{TotalPages}";

        [RelayCommand]
        private async Task FirstPage()
        {
            pageIndex = 1;

            await LoadConsumedMaterial();
        }

        [RelayCommand]
        private async Task NextPage()
        {
            if (pageIndex == TotalPages)
            {
                return;
            }
            pageIndex += 1;
            await LoadConsumedMaterial();
        }

        [RelayCommand]
        private async Task PreviousPage()
        {
            if (pageIndex == 1)
            {
                return;
            }
            pageIndex -= 1;
            await LoadConsumedMaterial();
        }

        [RelayCommand]
        private async Task LastPage()
        {
            pageIndex = TotalPages;
            await LoadConsumedMaterial();
        }

        #endregion Phan Trang ConsumedMaterial
    }
}
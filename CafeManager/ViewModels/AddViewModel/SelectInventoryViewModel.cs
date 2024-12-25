using AutoMapper;
using CafeManager.Core.Data;
using CafeManager.Core.DTOs;
using CafeManager.Core.Services;
using CafeManager.WPF.MessageBox;
using CafeManager.WPF.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using PdfSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CafeManager.WPF.ViewModels.AddViewModel
{
    public partial class SelectInventoryViewModel : ObservableObject, IDisposable, IDataViewModel
    {
        private readonly MaterialSupplierServices _materialSupplierServices;
        private readonly IMapper _mapper;

        [ObservableProperty]
        private bool _isLoading;

        [ObservableProperty]
        private List<MaterialSupplierDTO> _listInventoryDTO = [];

        [ObservableProperty]
        private ObservableCollection<MaterialDTO> _listMaterialDTO = [];

        [ObservableProperty]
        private ObservableCollection<SupplierDTO> _listSupplierDTO = [];

        [ObservableProperty]
        private MaterialSupplierDTO _selectedInventory = new();

        public event Action Commit;

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
                    FirstPageCommand.ExecuteAsync(null);
                }
            }
        }

        private SupplierDTO? _selectedSupplier;

        public SupplierDTO? SelectedSupplier
        {
            get => _selectedSupplier;
            set
            {
                if (_selectedSupplier != value)
                {
                    _selectedSupplier = value;
                    OnPropertyChanged(nameof(SelectedSupplier));
                    FirstPageCommand.ExecuteAsync(null);
                }
            }
        }

        #endregion Filter Declare

        public SelectInventoryViewModel(IServiceScope scope)
        {
            var provider = scope.ServiceProvider;
            _materialSupplierServices = provider.GetRequiredService<MaterialSupplierServices>();
            _mapper = provider.GetRequiredService<IMapper>();
            _ = LoadData();
        }

        public async Task LoadData(CancellationToken token = default)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                Expression<Func<Materialsupplier, bool>> filter = inventory =>
                (inventory.Isdeleted == false) &&
                (SelectedMaterial == null || inventory.Materialid == SelectedMaterial.Materialid) &&
                (SelectedSupplier == null || inventory.Supplierid == SelectedSupplier.Supplierid) &&
                inventory.Expirationdate > DateTime.Now;


                var dbListInventory = await _materialSupplierServices.GetSearchPaginateListMaterialsupplierAlter(filter, pageIndex, pageSize);
                ListInventoryDTO = [.. _mapper.Map<List<MaterialSupplierDTO>>(dbListInventory.Item1)];
                totalPages = (dbListInventory.Item2 + pageSize - 1) / pageSize;
                OnPropertyChanged(nameof(PageUI));
            }
            catch (OperationCanceledException)
            {
                Debug.WriteLine("LoadData của SelectInventoryViewModel bị hủy");
            }
        }

        public void ReceiveListMaterial(List<MaterialDTO> materials) => ListMaterialDTO = [.. materials];
        public void ReceiveListSupplier(List<SupplierDTO> suppliers) => ListSupplierDTO = [.. suppliers];

        public void ClearValue()
        {
            SelectedInventory = new();
            SelectedMaterial = null;
            SelectedSupplier = null;
        }

        [RelayCommand]
        private void ChooseItem(MaterialSupplierDTO inventory)
        {
            var res = MyMessageBox.ShowDialog("Bạn muốn lấy vật liệu này?", MyMessageBox.Buttons.Yes_No, MyMessageBox.Icons.Question);
            if (res.Equals("1"))
            {
                SelectedInventory = inventory.Clone();
                Commit?.Invoke();
            }
            
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #region Phan Trang ConsumedMaterial
        private int pageIndex = 1;

        private int pageSize = 6;
        private int totalPages = 0;

        public string PageUI => $"{pageIndex}/{totalPages}";

        [RelayCommand]
        private async Task FirstPage()
        {
            pageIndex = 1;
            IsLoading = true;
            await LoadData();
            IsLoading = false;
        }

        [RelayCommand]
        private async Task NextPage()
        {
            if (pageIndex == totalPages)
            {
                return;
            }
            pageIndex += 1;
            IsLoading = true;
            await LoadData();
            IsLoading = false;
        }

        [RelayCommand]
        private async Task PreviousPage()
        {
            if (pageIndex == 1)
            {
                return;
            }
            pageIndex -= 1;
            IsLoading = true;
            await LoadData();
            IsLoading = false;
        }

        [RelayCommand]
        private async Task LastPage()
        {
            pageIndex = totalPages;
            IsLoading = true;
            await LoadData();
            IsLoading = false;
        }
        #endregion

    }
}

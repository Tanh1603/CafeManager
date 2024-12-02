using AutoMapper;
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
    public partial class InvoiceViewModel : ObservableObject
    {
        private readonly IServiceProvider _provider;
        private readonly InvoiceServices _invoiceServices;
        private readonly IMapper _mapper;

        [ObservableProperty]
        private ModifyInvoiceViewModel _modifyInvoiceVM;

        [ObservableProperty]
        private bool _isOpenModifyInvoiceVM;

        [ObservableProperty]
        private ObservableCollection<InvoiceDTO> _listInvoiceDTO = [];

        private string _searchKey = string.Empty;

        public string SearchKey
        {
            get => _searchKey; set
            {
                if (_searchKey != value)
                {
                    _searchKey = value;
                }
            }
        }

        public InvoiceViewModel(IServiceProvider provider)
        {
            _provider = provider;
            _invoiceServices = provider.GetRequiredService<InvoiceServices>();
            _mapper = provider.GetRequiredService<IMapper>();

            ModifyInvoiceVM = provider.GetRequiredService<ModifyInvoiceViewModel>();
            Task.Run(LoadData);
        }

        private async Task LoadData()
        {
            //var dbListInvoice = await _invoiceServices.GetListInvoices();
            var dbListInvoice = await _invoiceServices.GetSearchPaginateListInvoice(null, pageIndex, pageSize);
            ListInvoiceDTO = [.. _mapper.Map<List<InvoiceDTO>>(dbListInvoice.Item1)];
            totalPages = (dbListInvoice.Item2 + pageSize - 1) / pageSize;
            OnPropertyChanged(nameof(PageUI));
        }

        [RelayCommand]
        private void UpdateInvoice(InvoiceDTO invoiceDTO)
        {
            IsOpenModifyInvoiceVM = true;
            ModifyInvoiceVM.RecieveInvoiceDTO(invoiceDTO.Clone());
        }

        [RelayCommand]
        private async Task DeleteInvoice(InvoiceDTO invoiceDTO)
        {
            try
            {
                string messageBox = MyMessageBox.ShowDialog("Bạn có muốn xóa hóa đơn này không?", MyMessageBox.Buttons.Yes_No, MyMessageBox.Icons.Question);
                if (messageBox.Equals("1"))
                {
                    bool isSuccessDeleted = await _invoiceServices.DeleteInvoice(invoiceDTO.Invoiceid);
                    if (isSuccessDeleted)
                    {
                        MyMessageBox.Show("Xóa hóa đơn thành công");
                        ListInvoiceDTO.Remove(invoiceDTO);
                    }
                }
            }
            catch (InvalidOperationException ioe)
            {
                MyMessageBox.Show(ioe.Message);
            }
        }

        [RelayCommand]
        private void CloseModifyInvoiceView()
        {
            IsOpenModifyInvoiceVM = false;
        }

        #region Phân trang

        private int pageIndex = 1;

        private int pageSize = 2;
        private int totalPages = 0;

        public string PageUI => $"{pageIndex}/{totalPages}";

        [RelayCommand]
        private async Task FirstPage()
        {
            pageIndex = 1;

            await LoadData();
        }

        [RelayCommand]
        private async Task NextPage()
        {
            if (pageIndex == totalPages)
            {
                return;
            }
            pageIndex += 1;
            await LoadData();
        }

        [RelayCommand]
        private async Task PreviousPage()
        {
            if (pageIndex == 1)
            {
                return;
            }
            pageIndex -= 1;
            await LoadData();
        }

        [RelayCommand]
        private async Task LastPage()
        {
            pageIndex = totalPages;
            await LoadData();
        }

        #endregion Phân trang
    }
}
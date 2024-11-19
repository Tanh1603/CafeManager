using CafeManager.Core.Data;
using CafeManager.Core.DTOs;
using CafeManager.Core.Services;
using CafeManager.WPF.MessageBox;
using CafeManager.WPF.Services;
using CafeManager.WPF.ViewModels.AddViewModel;
using CommunityToolkit.Mvvm.Collections;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CafeManager.WPF.ViewModels.AdminViewModel
{
    public partial class InvoiceViewModel : ObservableObject
    {
        private readonly IServiceProvider _provider;
        private readonly InvoiceServices _invoiceServices;

        [ObservableProperty]
        private ModifyInvoiceViewModel _modifyInvoiceVM;

        [ObservableProperty]
        private bool _isOpenModifyInvoiceVM;

        [ObservableProperty]
        private ObservableCollection<InvoiceDTO> _listInvoiceDTO = [];

        public InvoiceViewModel(IServiceProvider provider)
        {
            _provider = provider;
            _invoiceServices = provider.GetRequiredService<InvoiceServices>();
            ModifyInvoiceVM = provider.GetRequiredService<ModifyInvoiceViewModel>();

            Task.Run(LoadData);
        }

        private async Task LoadData()
        {
            var dbListInvoice = await _invoiceServices.GetListInvoices();
            ListInvoiceDTO = [.. dbListInvoice.Select(x => InvoiceMapper.ToDTO(x))];
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
                string messageBox = MyMessageBox.ShowDialog("Bạn có muốn xóa hóa đơn này không?");
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
    }
}
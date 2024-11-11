using CafeManager.Core.Data;
using CafeManager.WPF.Services;
using CommunityToolkit.Mvvm.Collections;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CafeManager.WPF.ViewModels.AdminViewModel
{
    public partial class InvoiceViewModel : ObservableObject
    {
        private readonly IServiceProvider _provider;
        private readonly InvoiceServices _invoiceServices;

        private Invoice CurrentInvoice;

        [ObservableProperty]
        private ObservableCollectionListSource<Invoice> _listInvoice;

        [ObservableProperty]
        private ObservableCollectionListSource<Invoicedetail> _listInvoiceDetails;

        [ObservableProperty]
        private decimal _totalPrice;

        [ObservableProperty]
        private DateTime _paymentstartdate = new();

        [ObservableProperty]
        private DateTime? _paymentenddate = new();

        [ObservableProperty]
        private string _paymentstatus = string.Empty;

        [ObservableProperty]
        private string _paymentmethod = string.Empty;

        [ObservableProperty]
        private decimal? _discountinvoice = new();

        [ObservableProperty]
        private bool _isOpenUpdateInvoice = new();

        public InvoiceViewModel(IServiceProvider provider)
        {
            _provider = provider;
            _invoiceServices = provider.GetRequiredService<InvoiceServices>();
            _ = LoadData();
        }

        private async Task LoadData()
        {
            ListInvoice = new(await _invoiceServices.GetListInvoices());
        }

        [RelayCommand]
        private async Task UpdateInvoice(Invoice invoice)
        {
            IsOpenUpdateInvoice = true;
            CurrentInvoice = invoice;

            Paymentstartdate = invoice.Paymentstartdate;
            Paymentenddate = invoice.Paymentenddate;
            Paymentstatus = invoice.Paymentstatus;
            Paymentmethod = invoice.Paymentmethod;
            Discountinvoice = invoice.Discountinvoice;

            ListInvoiceDetails = new(await _invoiceServices.GetListIvoiceDetailByInvoiceId(invoice.Invoiceid) ?? Enumerable.Empty<Invoicedetail>());

            decimal? totalPrice = await _invoiceServices.GetTotalPriceByInvoiceId(invoice.Invoiceid);
            TotalPrice = totalPrice.HasValue ? Math.Round(totalPrice.Value, 2) : decimal.Zero;
        }

        [RelayCommand]
        private async Task DeleteInvoice(Invoice invoice)
        {
            try
            {
                var deltedInvoice = await _invoiceServices.DeleteInvoice(invoice.Invoiceid);
                if (deltedInvoice)
                {
                    ListInvoice.Remove(invoice);
                    MessageBox.Show("Xóa món ăn thành công");
                }
                else
                {
                    MessageBox.Show("Lỗi");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        [RelayCommand]
        private void Submit()
        {
            try
            {
                CurrentInvoice.Paymentstartdate = Paymentstartdate;
                CurrentInvoice.Paymentenddate = Paymentenddate;
                CurrentInvoice.Paymentstatus = Paymentstatus;
                CurrentInvoice.Discountinvoice = Discountinvoice;
                CurrentInvoice.Paymentmethod = Paymentmethod;

                Invoice? invoice = _invoiceServices.UpdateInvoice(CurrentInvoice);
                if (invoice != null)
                {
                    var res = ListInvoice.FirstOrDefault(x => x.Invoiceid == CurrentInvoice.Invoiceid);
                    res = CurrentInvoice;
                    ListInvoice = new(ListInvoice);
                    MessageBox.Show("Sửa hóa đơn thành công");
                    IsOpenUpdateInvoice = false;
                }
                else
                {
                    MessageBox.Show("Lỗi");
                }
            }
            catch (InvalidOperationException ioe)
            {
                MessageBox.Show(ioe.Message);
            }
        }

        [RelayCommand]
        private void CloseUpdateInvoice()
        {
            Paymentstartdate = new();
            Paymentenddate = null;
            Paymentstatus = string.Empty;
            Paymentmethod = string.Empty;
            Discountinvoice = decimal.Zero;

            IsOpenUpdateInvoice = false;
        }
    }
}
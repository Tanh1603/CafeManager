using CafeManager.Core.Data;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace CafeManager.Core.DTOs
{
    public class InvoiceDTO : INotifyPropertyChanged
    {
        private int? _coffeetableid;

        public int? Coffeetableid
        {
            get => _coffeetableid;
            set
            {
                _coffeetableid = value;
                OnPropertyChanged(nameof(Coffeetableid));
            }
        }

        private DateTime _paymentstartdate = DateTime.Now;

        public DateTime Paymentstartdate
        {
            get => _paymentstartdate;
            set
            {
                _paymentstartdate = value;
                OnPropertyChanged(nameof(Paymentstartdate));
            }
        }

        private DateTime? _paymentenddate;

        public DateTime? Paymentenddate
        {
            get => _paymentenddate;
            set
            {
                _paymentenddate = value;
                OnPropertyChanged(nameof(Paymentenddate));
            }
        }

        private string _paymentstatus;

        public string Paymentstatus
        {
            get => _paymentstatus;
            set
            {
                _paymentstatus = value;
                OnPropertyChanged(nameof(Paymentstatus));
            }
        }

        private string _paymentmethod;

        public string Paymentmethod
        {
            get => _paymentmethod;
            set
            {
                _paymentmethod = value;
                OnPropertyChanged(nameof(Paymentmethod));
            }
        }

        private decimal? _discountinvoice;

        public decimal? Discountinvoice
        {
            get => _discountinvoice;
            set
            {
                _discountinvoice = value;
                OnPropertyChanged(nameof(Discountinvoice));
            }
        }

        private ObservableCollection<InvoiceDetailDTO> _listInvoiceDTO = new ObservableCollection<InvoiceDetailDTO>();

        public ObservableCollection<InvoiceDetailDTO> ListInvoiceDTO
        {
            get => _listInvoiceDTO;
            set
            {
                _listInvoiceDTO = value;
                OnPropertyChanged(nameof(ListInvoiceDTO));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public decimal? CaculateTotalPrice()
        {
            return ListInvoiceDTO?.Sum(x =>
            {
                decimal? discountInvoice = (100 - Discountinvoice) / 100 ?? 1;
                decimal foodPrice = x.Price ?? 0;
                decimal foodDiscount = (100 - (x.DiscountFood ?? 0)) / 100;
                int quantity = x.Quantity;

                return discountInvoice * foodDiscount * foodPrice * quantity;
            }) ?? 0;
        }
    }
}
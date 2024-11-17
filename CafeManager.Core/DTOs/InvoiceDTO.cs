using CafeManager.Core.Data;
using CafeManager.Core.Services;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CafeManager.Core.DTOs
{
    public class InvoiceDTO : INotifyPropertyChanged
    {
        private int _invoiceid;

        public int Invoiceid
        {
            get => _invoiceid;
            set
            {
                _invoiceid = value;
                OnPropertyChanged();
            }
        }

        private int? _coffeetableid;

        public int? Coffeetableid
        {
            get => _coffeetableid;
            set
            {
                _coffeetableid = value;
                OnPropertyChanged();
            }
        }

        private DateTime _paymentstartdate = DateTime.Now;

        public DateTime Paymentstartdate
        {
            get => _paymentstartdate;
            set
            {
                _paymentstartdate = value;
                OnPropertyChanged();
            }
        }

        private DateTime? _paymentenddate;

        public DateTime? Paymentenddate
        {
            get => _paymentenddate;
            set
            {
                _paymentenddate = value;
                OnPropertyChanged();
            }
        }

        private string _paymentstatus;

        public string Paymentstatus
        {
            get => _paymentstatus;
            set
            {
                _paymentstatus = value;
                OnPropertyChanged();
            }
        }

        private string _paymentmethod;

        public string Paymentmethod
        {
            get => _paymentmethod;
            set
            {
                _paymentmethod = value;
                OnPropertyChanged();
            }
        }

        private decimal _discountinvoice;

        public decimal Discountinvoice
        {
            get => _discountinvoice;
            set
            {
                _discountinvoice = value;
                OnPropertyChanged();
            }
        }

        private bool? _isdeleted;

        public bool? Isdeleted
        {
            get => _isdeleted;
            set
            {
                _isdeleted = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<InvoiceDetailDTO> _listInvoiceDTO = [];

        public ObservableCollection<InvoiceDetailDTO> ListInvoiceDTO
        {
            get => _listInvoiceDTO;
            set
            {
                _listInvoiceDTO = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(CaculateTotalPrice));
            }
        }

        private StaffDTO _staffDTO;

        public StaffDTO StaffDTO
        {
            get => _staffDTO;
            set
            {
                _staffDTO = value;
                OnPropertyChanged();
            }
        }

        private int _staffid;

        public int Staffid
        {
            get => _staffid;
            set
            {
                _staffid = value;
                OnPropertyChanged();
            }
        }

        public InvoiceDTO Clone()
        {
            return new InvoiceDTO()
            {
                Invoiceid = this.Invoiceid,
                Coffeetableid = this.Coffeetableid,
                Paymentstartdate = this.Paymentstartdate,
                Paymentenddate = this.Paymentenddate,
                Paymentstatus = this.Paymentstatus,
                Paymentmethod = this.Paymentmethod,
                Discountinvoice = this.Discountinvoice,
                Isdeleted = this.Isdeleted,
                Staffid = this.Staffid,

                StaffDTO = this.StaffDTO,
            };
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public decimal CaculateTotalPrice()
        {
            return ListInvoiceDTO?.Sum(x =>
            {
                decimal? discountInvoice = (100 - Discountinvoice) / 100;
                decimal foodPrice = x.FoodDTO.Price;
                decimal foodDiscount = (100 - x.FoodDTO.Discountfood) / 100;
                int quantity = x.Quantity;

                return discountInvoice * foodDiscount * foodPrice * quantity;
            }) ?? 0;
        }
    }
}
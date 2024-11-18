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

        private ObservableCollection<InvoiceDetailDTO> _listInvoiceDetailDTO = [];

        public ObservableCollection<InvoiceDetailDTO> ListInvoiceDetailDTO
        {
            get => _listInvoiceDetailDTO;
            set
            {
                _listInvoiceDetailDTO = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(TotalPrice));
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

        private CoffeetableDTO _coffeetableDTO;

        public CoffeetableDTO CoffeetableDTO
        {
            get => _coffeetableDTO;
            set
            {
                _coffeetableDTO = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(CustomerOrTable));
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
                CoffeetableDTO = this.CoffeetableDTO,
                ListInvoiceDetailDTO = this.ListInvoiceDetailDTO,
            };
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private bool _isCoffeeTable = false;
        private bool _isCustomer = false;
        public bool IsCoffeeTable { get => _isCoffeeTable; set => _isCoffeeTable = value; }
        public bool IsCustomer { get => _isCustomer; set => _isCustomer = value; }

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public decimal TotalPrice => CaculateTotalPrice();

        public string CustomerOrTable => CoffeetableDTO != null ? CoffeetableDTO.TableName : "Khác vãng lai";

        public decimal CaculateTotalPrice()
        {
            return ListInvoiceDetailDTO?.Sum(x =>
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
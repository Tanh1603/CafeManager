using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

#nullable disable

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

        private ObservableCollection<InvoiceDetailDTO> _invoicedetails = [];

        public ObservableCollection<InvoiceDetailDTO> Invoicedetails
        {
            get => _invoicedetails;
            set
            {
                if (_invoicedetails != value)
                {
                    _invoicedetails = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(TotalPrice));
                }
            }
        }

        private StaffDTO _staff;

        public StaffDTO Staff
        {
            get => _staff;
            set
            {
                if (_staff != value)
                {
                    _staff = value;
                    OnPropertyChanged();
                }
            }
        }

        private CoffeetableDTO _coffeetable;

        public CoffeetableDTO Coffeetable
        {
            get => _coffeetable;
            set
            {
                if (_coffeetable != value)
                {
                    _coffeetable = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(CustomerOrTable));
                }
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
                Invoiceid = Invoiceid,
                Coffeetableid = Coffeetableid,
                Paymentstartdate = Paymentstartdate,
                Paymentenddate = Paymentenddate,
                Paymentstatus = Paymentstatus,
                Paymentmethod = Paymentmethod,
                Discountinvoice = Discountinvoice,
                Isdeleted = Isdeleted,
                Staffid = Staffid,

                Staff = Staff,
                Coffeetable = Coffeetable,
                Invoicedetails = Invoicedetails,
            };
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private bool _isCoffeeTable = false;
        private bool _isCustomer = false;
        public bool IsCoffeeTable { get => _isCoffeeTable; set => _isCoffeeTable = value; }

        public bool IsCustomer
        {
            get => _isCustomer;
            set
            {
                _isCustomer = value;
                OnPropertyChanged();
            }
        }

        private readonly string _invoiceCustomerId;
        public string InvoiceCustomerId => _invoiceCustomerId;

        public InvoiceDTO()
        {
            _invoiceCustomerId = $"HD-{Guid.NewGuid().ToString().Substring(0, 4).ToUpper()}";
        }

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public decimal TotalPrice => CaculateTotalPrice();

        public string CustomerOrTable => Coffeetable != null ? Coffeetable.TableName : "Khác vãng lai";

        public decimal CaculateTotalPrice()
        {
            return Invoicedetails?.Sum(x =>
            {
                decimal? discountInvoice = (100 - Discountinvoice) / 100;
                decimal foodPrice = x.Food.Price;
                decimal foodDiscount = (100 - x.Food.Discountfood) / 100;
                int quantity = x.Quantity;

                return discountInvoice * foodDiscount * foodPrice * quantity;
            }) ?? 0;
        }
    }
}
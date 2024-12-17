using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

#nullable disable

namespace CafeManager.Core.DTOs
{
    public partial class InvoiceDTO : BaseDTO
    {
        [ObservableProperty]
        private int _invoiceid;

        [ObservableProperty]
        private int? _coffeetableid;

        [ObservableProperty]
        private DateTime _paymentstartdate = DateTime.Now;

        [ObservableProperty]
        private string _paymentstatus;

        [ObservableProperty]
        private string _paymentmethod;

        [ObservableProperty]
        private decimal _discountinvoice;

        [ObservableProperty]
        private DateTime? _paymentenddate;

        [ObservableProperty]

        [NotifyPropertyChangedFor(nameof(TotalPrice))]
        private ObservableCollection<InvoiceDetailDTO> _invoicedetails = [];

        [ObservableProperty]
        private StaffDTO _staff;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(CustomerOrTable))]
        private CoffeetableDTO _coffeetable;

        [ObservableProperty]
        private int _staffid;

        public InvoiceDTO Clone()
        {
            return new InvoiceDTO()
            {
                Id = Id,
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

        public string InvoiceCustomerId => $"HD-{Id.ToString().Substring(0, 4).ToUpper()}";

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
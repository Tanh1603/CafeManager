using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

#nullable disable

namespace CafeManager.Core.DTOs
{
    public partial class InvoiceDetailDTO : BaseDTO
    {
        [ObservableProperty]
        private int _invoicedetailid;

        [ObservableProperty]
        private int _invoiceid;

        [ObservableProperty]
        private int _foodid;

        [ObservableProperty]
        private FoodDTO _food;

        private int _quantity;

        public int Quantity
        {
            get => _quantity;
            set
            {
                SetProperty(ref _quantity, value, true);
                QuantityChanged?.Invoke();
            }
        }

        public event Action QuantityChanged;

        public InvoiceDetailDTO Clone()
        {
            return new InvoiceDetailDTO()
            {
                Id = Id,
                Invoicedetailid = Invoicedetailid,
                Invoiceid = Invoiceid,
                Foodid = Foodid,
                Quantity = Quantity,
                Isdeleted = Isdeleted,
                Food = Food,
            };
        }
    }
}
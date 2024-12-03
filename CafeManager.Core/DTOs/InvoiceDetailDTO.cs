using System.ComponentModel;
using System.Runtime.CompilerServices;

#nullable disable

namespace CafeManager.Core.DTOs
{
    public class InvoiceDetailDTO : BaseDTO
    {
        private int _invoicedetailid;
        private int _invoiceid;
        private int _foodid;
        private int _quantity;
        private bool _isdeleted;
        private FoodDTO _food;

        public int Invoicedetailid
        {
            get => _invoicedetailid;
            set
            {
                _invoicedetailid = value;
                OnPropertyChanged();
            }
        }

        public int Invoiceid
        {
            get => _invoiceid;
            set
            {
                _invoiceid = value;
                OnPropertyChanged();
            }
        }

        public int Foodid
        {
            get => _foodid;
            set
            {
                _foodid = value;
                OnPropertyChanged();
            }
        }

        public bool Isdeleted
        {
            get => _isdeleted;
            set
            {
                _isdeleted = value;
                OnPropertyChanged();
            }
        }

        public int Quantity
        {
            get => _quantity;
            set
            {
                if (_quantity != value)
                {
                    _quantity = value;
                    OnPropertyChanged();
                    QuantityChanged?.Invoke();
                }
            }
        }

        public event Action QuantityChanged;

        public FoodDTO Food
        {
            get => _food;
            set
            {
                if (_food != value)
                {
                    _food = value;
                    OnPropertyChanged();
                }
            }
        }

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
using CafeManager.Core.Data;
using CafeManager.Core.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CafeManager.Core.DTOs
{
    public class InvoiceDetailDTO : INotifyPropertyChanged
    {
        private int _invoicedetailid;
        private int? _invoiceid;
        private int _foodid;
        private int _quantity;
        private bool? _isdeleted;
        private FoodDTO _foodDTO;

        public int Invoicedetailid
        {
            get => _invoicedetailid;
            set
            {
                _invoicedetailid = value;
                OnPropertyChanged();
            }
        }

        public int? Invoiceid
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

        public bool? Isdeleted
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

        public event Action? QuantityChanged;

        public FoodDTO FoodDTO
        {
            get => _foodDTO;
            set
            {
                _foodDTO = value;
                OnPropertyChanged();
            }
        }

        public InvoiceDetailDTO Clone()
        {
            return new InvoiceDetailDTO()
            {
                Invoicedetailid = this.Invoicedetailid,
                Invoiceid = this.Invoiceid,
                Foodid = this.Foodid,
                Quantity = this.Quantity,
                Isdeleted = this.Isdeleted,
                FoodDTO = this.FoodDTO,
            };
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
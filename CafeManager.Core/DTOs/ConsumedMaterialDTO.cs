using System.ComponentModel;
using System.Runtime.CompilerServices;

#nullable disable

namespace CafeManager.Core.DTOs
{
    public class ConsumedMaterialDTO : BaseDTO
    {
        private int _consumedmaterialid;
        private int _materialsuppplierid;
        private decimal _quantity;
        private bool _isdeleted;
        private MaterialSupplierDTO _materialsupplier;
        private DateOnly _usagedate;

        public int Consumedmaterialid
        {
            get => _consumedmaterialid; set
            {
                _consumedmaterialid = value;
                OnPropertyChanged();
            }
        }

        public int Materialsupplierid
        {
            get => _materialsuppplierid; set
            {
                _materialsuppplierid = value;
                OnPropertyChanged();
            }
        }

        public decimal Quantity
        {
            get => _quantity; set
            {
                _quantity = value;
                OnPropertyChanged();
            }
        }

        public bool Isdeleted
        {
            get => _isdeleted; set
            {
                _isdeleted = value;
                OnPropertyChanged();
            }
        }

        public MaterialSupplierDTO Materialsupplier
        {
            get => _materialsupplier; set
            {
                if (_materialsupplier != value)
                {
                    _materialsupplier = value;
                    OnPropertyChanged();
                }
            }
        }

        public DateOnly Usagedate
        {
            get => _usagedate; set
            {
                if (_usagedate != value)
                {
                    _usagedate = value;
                    OnPropertyChanged();
                }
            }
        }

        public ConsumedMaterialDTO Clone()
        {
            return new ConsumedMaterialDTO
            {
                Consumedmaterialid = Consumedmaterialid,
                Materialsupplierid = Materialsupplierid,
                Quantity = Quantity,
                Isdeleted = Isdeleted,
                Materialsupplier = Materialsupplier
            };
        }
    }
}
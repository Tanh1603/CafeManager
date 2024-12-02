using CafeManager.Core.Data;
using System.ComponentModel;
using System.Runtime.CompilerServices;

#nullable disable

namespace CafeManager.Core.DTOs
{
    public class ImportDetailDTO : BaseDTO
    {
        private int _importdetailid;
        private int _importid;
        private int _materialsupplierid;
        private decimal _quantity;
        private bool _isdeleted;
        private MaterialSupplierDTO _materialsupplier;
        private ImportDTO _import;

        public int Importdetailid
        {
            get => _importdetailid;
            set
            {
                if (_importdetailid != value)
                {
                    _importdetailid = value;
                    OnPropertyChanged();
                }
            }
        }

        public int Importid
        {
            get => _importid;
            set
            {
                _importid = value;
                OnPropertyChanged();
            }
        }

        public int Materialid
        {
            get => _materialsupplierid;
            set
            {
                _materialsupplierid = value;
                OnPropertyChanged();
            }
        }

        public decimal Quantity
        {
            get => _quantity;
            set
            {
                if (_quantity != value)
                {
                    _quantity = value;
                    OnPropertyChanged();
                    UpdateTotalPriceAction?.Invoke();
                }
            }
        }

        public bool Isdeleted
        {
            get => _isdeleted;
            set
            {
                if (_isdeleted != value)
                {
                    _isdeleted = value;
                    OnPropertyChanged();
                }
            }
        }

        public ImportDTO Import
        {
            get => _import;
            set
            {
                if (_import != value)
                {
                    _import = value;
                    OnPropertyChanged();
                }
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

        public ImportDetailDTO Clone()
        {
            return new ImportDetailDTO
            {
                Id = Id,
                Importdetailid = Importdetailid,
                Importid = Importid,
                Materialid = Materialid,
                Isdeleted = Isdeleted,
                Quantity = Quantity,

                Import = Import,
                Materialsupplier = Materialsupplier,
            };
        }

        public static Action UpdateTotalPriceAction { get; set; }
    }
}
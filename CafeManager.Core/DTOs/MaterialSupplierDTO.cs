using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

#nullable disable

namespace CafeManager.Core.DTOs
{
    public class MaterialSupplierDTO : BaseDTO
    {
        private int _materialsupplierid;
        private int materialid;
        private int supplierid;
        private DateTime _manufacturedate;
        private DateTime _expirationdate;
        private string _original;
        private string _manufacturer;
        private decimal _price;
        private bool _isdeleted;

        private MaterialDTO _material;
        private SupplierDTO _supplier;
        private ObservableCollection<ConsumedMaterialDTO> _consumedmaterials;

        private decimal _totalQuantity;

        public int Materialsupplierid
        {
            get => _materialsupplierid;
            set
            {
                if (_materialsupplierid != value)
                {
                    _materialsupplierid = value;
                    OnPropertyChanged();
                }
            }
        }

        public int Materialid
        {
            get => materialid; set
            {
                materialid = value;
                OnPropertyChanged();
            }
        }

        public int Supplierid
        {
            get => supplierid; set
            {
                supplierid = value;
                OnPropertyChanged();
            }
        }

        public DateTime Manufacturedate
        {
            get => _manufacturedate;
            set
            {
                if (_manufacturedate != value)
                {
                    _manufacturedate = value;
                    OnPropertyChanged();
                }
            }
        }

        public DateTime Expirationdate
        {
            get => _expirationdate;
            set
            {
                if (_expirationdate != value)
                {
                    _expirationdate = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Original
        {
            get => _original;
            set
            {
                if (_original != value)
                {
                    _original = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Manufacturer
        {
            get => _manufacturer;
            set
            {
                if (_manufacturer != value)
                {
                    _manufacturer = value;
                    OnPropertyChanged();
                }
            }
        }

        public decimal Price
        {
            get => _price;
            set
            {
                if (_price != value)
                {
                    _price = value;
                    OnPropertyChanged();
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

        public decimal TotalQuantity
        {
            get => _totalQuantity;
            set
            {
                if (value != _totalQuantity)
                {
                    _totalQuantity = value;
                    OnPropertyChanged();
                }
            }
        }

        public MaterialDTO Material
        {
            get => _material; set
            {
                if (_material != value)
                {
                    _material = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(TotalQuantity));
                }
            }
        }

        public SupplierDTO Supplier
        {
            get => _supplier; set
            {
                if (_supplier != value)
                {
                    _supplier = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(TotalQuantity));
                }
            }
        }

        public MaterialSupplierDTO Clone()
        {
            return new MaterialSupplierDTO()
            {
                Id = Id,
                Materialsupplierid = Materialsupplierid,
                Materialid = Materialid,
                Supplierid = Supplierid,
                Manufacturedate = Manufacturedate,
                Expirationdate = Expirationdate,
                Original = Original,
                Manufacturer = Manufacturer,
                Price = Price,
                Isdeleted = Isdeleted,

                Material = Material,
                Supplier = Supplier,
                TotalQuantity = TotalQuantity,
                Consumedmaterials = Consumedmaterials,
            };
        }

        public ObservableCollection<ConsumedMaterialDTO> Consumedmaterials
        {
            get => _consumedmaterials; set
            {
                if (_consumedmaterials != value)
                {
                    _consumedmaterials = value;
                    OnPropertyChanged();
                }
            }
        }
    }
}
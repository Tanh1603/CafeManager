﻿using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

#nullable disable

namespace CafeManager.Core.DTOs
{
    public class MaterialSupplierDTO : INotifyPropertyChanged
    {
        private int _materialsupplierid;
        private int materialid;
        private int supplierid;
        private DateTime _manufacturedate;
        private DateTime _expirationdate;
        private string _original;
        private string _manufacturer;
        private decimal _price;
        private bool? _isdeleted;

        private MaterialDTO _materialDTO;
        private SupplierDTO _supplierDTO;
        private ObservableCollection<ConsumedMaterialDTO> _consumedMaterialDTO;

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

        public bool? Isdeleted
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

        public MaterialDTO MaterialDTO
        {
            get => _materialDTO; set
            {
                _materialDTO = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(TotalQuantity));
            }
        }

        public SupplierDTO SupplierDTO
        {
            get => _supplierDTO; set
            {
                _supplierDTO = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(TotalQuantity));
            }
        }

        public MaterialSupplierDTO Clone()
        {
            return new MaterialSupplierDTO()
            {
                Materialsupplierid = this.Materialsupplierid,
                Materialid = this.Materialid,
                Supplierid = this.Supplierid,
                Manufacturedate = this.Manufacturedate,
                Expirationdate = this.Expirationdate,
                Original = this.Original,
                Manufacturer = this.Manufacturer,
                Price = this.Price,
                Isdeleted = this.Isdeleted,

                MaterialDTO = this.MaterialDTO,
                SupplierDTO = this.SupplierDTO
            };
        }

        public decimal TotalQuantity => MaterialDTO.ImportdetailDTO.Where(x => x.ImportDTO.Supplierid == SupplierDTO.Supplierid).Sum(x => x.Quantity);

        public ObservableCollection<ConsumedMaterialDTO> ConsumedMaterialDTO
        {
            get => _consumedMaterialDTO; set
            {
                _consumedMaterialDTO = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
using CafeManager.Core.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace CafeManager.Core.DTOs
{
    public class MaterialDetailDTO : INotifyPropertyChanged
    {
        private int _materialsupplierid;

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

        private Material? _currentmaterial;
        public Material? CurrentMaterial
        {
            get => _currentmaterial;
            set
            {
                if (_currentmaterial != value)
                {
                    _currentmaterial = value;
                    OnPropertyChanged();
                }
            }
        }

        //private string? _materialname;
        //public string? Materialname
        //{
        //    get => _materialname;
        //    set
        //    {
        //        if (_materialname != value)
        //        {
        //            _materialname = value;
        //            OnPropertyChanged();
        //        }
        //    }
        //}

        //private string? _unit;
        //public string? Unit
        //{
        //    get => _unit;
        //    set
        //    {
        //        if (_unit != value)
        //        {
        //            _unit = value;
        //            OnPropertyChanged();
        //        }
        //    }
        //}

        private decimal? _quantity;
        public decimal? Quantity
        {
            get => _quantity;
            set
            {
                if (_quantity != value)
                {
                    _quantity = value;
                    OnPropertyChanged();
                }
            }
        }

        private decimal? _price;
        public decimal? Price
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

        private string? _original;
        public string? Original
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

        private string? _manufacturer;

        public string? Manufacturer
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

        private DateTime _manufacturedate;
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

        private DateTime _expirationdate;

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

        private bool? _isdeleted;

        public MaterialDetailDTO()
        {
        }

        public MaterialDetailDTO(MaterialDetailDTO materialDetailDTO)
        {
            if (materialDetailDTO != null)
            {
                Materialsupplierid = materialDetailDTO.Materialsupplierid;
                CurrentMaterial = materialDetailDTO.CurrentMaterial;
                Quantity = materialDetailDTO.Quantity;
                Manufacturer = materialDetailDTO.Manufacturer;
                Original = materialDetailDTO.Original;
                Manufacturedate = materialDetailDTO.Manufacturedate;
                Expirationdate = materialDetailDTO.Expirationdate;
                Price = materialDetailDTO.Price;
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


        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CafeManager.Core.DTOs
{
    public class ImportMaterialDetailDTO
    {
        private int _importdetailid;
        private int _materialsupplierid;
        private int _materialid;
        private string _materialname;
        private string _unit;
        private decimal _quantity;
        private decimal _price;
        private string _original;
        private string _manufacturer;   
        private DateTime _manufacturedate;
        private DateTime _expirationdate;
        private bool _isdeleted;

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

        public int Materialsupplierid
        {
            get => _materialsupplierid;
            set
            {
                if(value != _materialsupplierid)
                {
                    _materialsupplierid = value;
                    OnPropertyChanged();
                }    
            }
        }

        public int Materialid
        {
            get => _materialid;
            set
            {
                if (value != _materialid)
                {
                    _materialid = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Materialname
        {
            get => _materialname;
            set
            {
                _materialname = value;
                OnPropertyChanged();
            }
        }
        public string Unit
        {
            get => _unit;
            set
            {
                _unit = value;
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
                    //UpdateTotalPriceAction?.Invoke();
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

        public event PropertyChangedEventHandler PropertyChanged;

        //public static Action UpdateTotalPriceAction { get; set; }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

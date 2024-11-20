using System.ComponentModel;
using System.Runtime.CompilerServices;

#nullable disable

namespace CafeManager.Core.DTOs
{
    public class ImportDetailDTO : INotifyPropertyChanged
    {
        private int _importdetailid;
        private int importid;
        private int materialid;
        private decimal _quantity;
        private bool? _isdeleted;
        private MaterialDTO _materialDTO;
        private ImportDTO _importDTO;

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
            get => importid;
            set
            {
                importid = value;
                OnPropertyChanged();
            }
        }

        public int Materialid
        {
            get => materialid;
            set
            {
                materialid = value;
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
            get => _materialDTO;
            set
            {
                _materialDTO = value;
                OnPropertyChanged();
            }
        }

        public ImportDTO ImportDTO
        {
            get => _importDTO; set
            {
                _importDTO = value;
                OnPropertyChanged();
            }
        }

        public ImportDetailDTO Clone()
        {
            return new ImportDetailDTO
            {
                Importdetailid = this.Importdetailid,
                Importid = this.Importid,
                Materialid = this.Materialid,
                Isdeleted = this.Isdeleted,
                Quantity = this.Quantity,

                MaterialDTO = this.MaterialDTO
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public static Action UpdateTotalPriceAction { get; set; }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
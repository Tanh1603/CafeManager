using System.ComponentModel;
using System.Runtime.CompilerServices;

#nullable disable

namespace CafeManager.Core.DTOs
{
    public class ConsumedMaterialDTO : INotifyPropertyChanged
    {
        private int _consumedmaterialid;
        private int? _materialid;
        private decimal _quantity;
        private bool? _isdeleted;
        private MaterialDTO _materialDTO;

        public int Consumedmaterialid
        {
            get => _consumedmaterialid; set
            {
                _consumedmaterialid = value;
                OnPropertyChanged();
            }
        }

        public int? Materialid
        {
            get => _materialid; set
            {
                _materialid = value;
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

        public bool? Isdeleted
        {
            get => _isdeleted; set
            {
                _isdeleted = value;
                OnPropertyChanged();
            }
        }

        public MaterialDTO MaterialDTO
        {
            get => _materialDTO; set
            {
                _materialDTO = value;
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
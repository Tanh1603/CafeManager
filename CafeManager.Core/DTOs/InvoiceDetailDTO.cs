using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CafeManager.Core.DTOs
{
    public class InvoiceDetailDTO : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public int InvoiceId { get; set; }
        public int FoodId { get; set; }

        public string? FoodName { get; set; }
        public decimal? Price { get; set; }
        public decimal? DiscountFood { get; set; }
        private int _quantity;

        public int Quantity
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

        public event PropertyChangedEventHandler? PropertyChanged;

        public static Action? UpdateTotalPriceAction { get; set; }

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
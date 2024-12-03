using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CafeManager.Core.DTOs
{
    public class BaseDTO : INotifyPropertyChanged
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
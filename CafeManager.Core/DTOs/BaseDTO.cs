using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CafeManager.Core.DTOs
{
    public partial class BaseDTO : ObservableValidator
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [ObservableProperty]
        private bool _isdeleted;

        public void ValidateDTO() => ValidateAllProperties();
    }
}
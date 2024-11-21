using CafeManager.Core.Data;
using CafeManager.Core.DTOs;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeManager.WPF.ViewModels.AddViewModel
{
    public partial class AddSuppierViewModel : ObservableObject
    {
        private readonly IServiceProvider _provider;

        public bool IsUpdating { get; set; } = false;
        public bool IsAdding { get; set; } = false;

        [ObservableProperty]
        private SupplierDTO _modifySupplier = new();

        public event Action<SupplierDTO>? ModifySupplierChanged;

        public AddSuppierViewModel(IServiceProvider provider)
        {
            _provider = provider;
        }

        public void RecieveSupplierDTO(SupplierDTO supplier)
        {
            ModifySupplier = supplier.Clone();
        }

        public void ClearValueOfFrom()
        {
            ModifySupplier = new();
            IsAdding = false;
            IsUpdating = false;
        }

        [RelayCommand]
        private void Submit()
        {
            ModifySupplierChanged?.Invoke(ModifySupplier.Clone());
            ClearValueOfFrom();
        }
    }
}
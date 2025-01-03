using CafeManager.Core.Data;
using CafeManager.Core.DTOs;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeManager.WPF.ViewModels.AddViewModel
{
    public partial class AddSuppierViewModel : ObservableValidator
    {
        private readonly IServiceProvider _provider;

        public bool IsUpdating { get; set; } = false;
        public bool IsAdding { get; set; } = false;

        [ObservableProperty]
        private SupplierDTO _modifySupplier;

        public event Action<SupplierDTO>? ModifySupplierChanged;

        public event Action Close;

        public AddSuppierViewModel(IServiceProvider provider)
        {
            _provider = provider;
            ModifySupplier = new SupplierDTO();
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
            ModifySupplier.ValidateDTO();
            if (ModifySupplier.HasErrors)
            {
                return;
            }
            ModifySupplierChanged?.Invoke(ModifySupplier.Clone());
            ClearValueOfFrom();
        }

        [RelayCommand]
        private void CloseUserControl()
        {
            Close?.Invoke();
        }
    }
}
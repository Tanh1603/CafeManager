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
    public partial class AddSuppierViewModel : ObservableObject
    {
        private readonly IServiceProvider _provider;
        private readonly ErrorViewModel _errorViewModel;
        public bool IsUpdating { get; set; } = false;
        public bool IsAdding { get; set; } = false;

        [ObservableProperty]
        private SupplierDTO _modifySupplier = new();

        public event Action<SupplierDTO>? ModifySupplierChanged;

        public event Action Close;

        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        public AddSuppierViewModel(IServiceProvider provider)
        {
            _provider = provider;
            _errorViewModel = new ErrorViewModel();
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

        [RelayCommand]
        private void CloseUserControl()
        {
            Close?.Invoke();
        }
    }
}
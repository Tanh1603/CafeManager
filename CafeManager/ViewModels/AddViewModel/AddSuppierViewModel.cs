using CafeManager.Core.Data;
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

        [ObservableProperty]
        private int _id;

        [ObservableProperty]
        private string _suppliername;

        [ObservableProperty]
        private string _representativesupplier;

        [ObservableProperty]
        private string _phone;

        [ObservableProperty]
        private string _email;

        [ObservableProperty]
        private string _address;

        [ObservableProperty]
        private string _notes;

        public bool IsUpdating { get; set; } = false;
        public bool IsAdding { get; set; } = false;

        public event Action<Supplier> AddSupplierChanged;

        public event Action<Supplier> UpdateSupplierNameChanged;

        public event Action Close;

        public AddSuppierViewModel(IServiceProvider provider)
        {
            _provider = provider;
        }

        public void HandleSupplierFromParent(Supplier supplier)
        {
            Id = supplier.Supplierid;
            Suppliername = supplier.Suppliername;
            Representativesupplier = supplier.Representativesupplier;
            Phone = supplier.Phone;
            Email = supplier.Email;
            Address = supplier.Address;
            Notes = supplier.Notes;
            IsUpdating = true;
        }

        [RelayCommand]
        private void Submit()
        {
            Supplier newSupplier = new()
            {
                Suppliername = this.Suppliername,
                Representativesupplier = this.Representativesupplier,
                Phone = this.Phone,
                Email = this.Email,
                Address = this.Address,
                Notes = this.Notes,
            };
            if (IsUpdating)
            {
                newSupplier.Supplierid = Id;
                UpdateSupplierNameChanged?.Invoke(newSupplier);
            }
            else if (IsAdding)
            {
                AddSupplierChanged?.Invoke(newSupplier);
            }
        }

        [RelayCommand]
        private void CloseUserControl()
        {
            Close?.Invoke();
        }
    }
}
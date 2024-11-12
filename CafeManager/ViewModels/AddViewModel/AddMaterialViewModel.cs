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
    public partial class AddMaterialViewModel : ObservableObject
    {
        private readonly IServiceProvider _provider;

        [ObservableProperty]
        private int _materialid;

        [ObservableProperty]
        private string _materialname;

        [ObservableProperty]
        private string _unit;

        public bool IsUpdating { get; set; } = false;
        public bool IsAdding { get; set; } = false;

        public event Action<Material> AddMaterialChanged;

        public event Action<Material> UpdateMaterialNameChanged;

        public event Action Close;

        public AddMaterialViewModel(IServiceProvider provider)
        {
            _provider = provider;
        }

        public void HandleSupplierFromParent(Material material)
        {
            Materialid = material.Materialid;
            Materialname = material.Materialname;
            Unit = material.Unit;
            IsUpdating = true;
        }

        [RelayCommand]
        private void Submit()
        {
            Material newMaterial = new()
            {
                Materialid = this.Materialid,
                Materialname = this.Materialname,
                Unit = this.Unit
            };
            if (IsUpdating)
            {
                newMaterial.Materialid = Materialid;
                UpdateMaterialNameChanged?.Invoke(newMaterial);
            }
            else if (IsAdding)
            {
                AddMaterialChanged?.Invoke(newMaterial);
            }
        }

        [RelayCommand]
        private void CloseUserControl()
        {
            Close?.Invoke();
        }
    }
}

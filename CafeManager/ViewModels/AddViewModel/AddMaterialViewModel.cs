﻿using CafeManager.Core.Data;
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
    public partial class AddMaterialViewModel : ObservableObject
    {
        private readonly IServiceProvider _provider;

        public bool IsUpdating { get; set; } = false;
        public bool IsAdding { get; set; } = false;

        [ObservableProperty]
        private MaterialDTO _modifyMaterial = new();

        public event Action<MaterialDTO> ModifyMaterialChanged;

        public event Action Close;

        public AddMaterialViewModel(IServiceProvider provider)
        {
            _provider = provider;
        }

        public void ReceiveMaterialDTO(MaterialDTO material)
        {
            ModifyMaterial = material.Clone();
        }

        public void ClearValueOfFrom()
        {
            ModifyMaterial = new();
            IsAdding = false;
            IsUpdating = false;
        }

        [RelayCommand]
        private void Submit()
        {
            ModifyMaterial.ValidateDTO();
            if (ModifyMaterial.HasErrors)
            {
                return;
            }
            ModifyMaterialChanged?.Invoke(ModifyMaterial.Clone());
            ClearValueOfFrom();
        }

        [RelayCommand]
        private void CloseUserControl()
        {
            Close?.Invoke();
        }
    }
}
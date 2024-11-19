﻿using CafeManager.Core.Data;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

#nullable disable

namespace CafeManager.Core.DTOs
{
    public class SupplierDTO : INotifyPropertyChanged
    {
        private int _supplierid;

        private string _suppliername;

        private string _representativesupplier;

        private string _phone;

        private string _email;

        private string _address;

        private string _notes;

        private bool? _isdeleted;

        private ObservableCollection<ImportDTO> _importDTO = [];
        private ObservableCollection<MaterialSupplierDTO> _materialsupplierDTO = [];

        public int Supplierid
        {
            get => _supplierid; set
            {
                _supplierid = value;
                OnPropertyChanged();
            }
        }

        public string Suppliername
        {
            get => _suppliername; set
            {
                _suppliername = value;
                OnPropertyChanged();
            }
        }

        public string Representativesupplier
        {
            get => _representativesupplier; set
            {
                _representativesupplier = value;
                OnPropertyChanged();
            }
        }

        public string Phone
        {
            get => _phone; set
            {
                _phone = value;
                OnPropertyChanged();
            }
        }

        public string Email
        {
            get => _email; set
            {
                _email = value;
                OnPropertyChanged();
            }
        }

        public string Address
        {
            get => _address; set
            {
                _address = value;
                OnPropertyChanged();
            }
        }

        public string Notes
        {
            get => _notes; set
            {
                _notes = value;
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

        public ObservableCollection<ImportDTO> ImportDTO
        {
            get => _importDTO; set
            {
                _importDTO = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<MaterialSupplierDTO> MaterialsupplierDTO
        {
            get => _materialsupplierDTO; set
            {
                _materialsupplierDTO = value;
                OnPropertyChanged();
            }
        }

        public SupplierDTO Clone()
        {
            return new SupplierDTO
            {
                Supplierid = Supplierid,
                Suppliername = Suppliername,
                Representativesupplier = Representativesupplier,
                Phone = Phone,
                Email = Email,
                Address = Address,
                Notes = Notes,
                Isdeleted = Isdeleted,

                ImportDTO = ImportDTO,
                MaterialsupplierDTO = MaterialsupplierDTO
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
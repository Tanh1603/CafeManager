﻿using System.Collections.ObjectModel;

#nullable disable

namespace CafeManager.Core.DTOs
{
    public class MaterialDTO : BaseDTO
    {
        private int materialid;
        private string materialname;
        private string unit;
        private bool isdeleted;

        private ObservableCollection<MaterialSupplierDTO> _materialsuppliers = [];
        private ObservableCollection<ImportDetailDTO> _importdetails = [];

        public int Materialid
        {
            get => materialid;
            set
            {
                materialid = value;
                OnPropertyChanged();
            }
        }

        public string Materialname
        {
            get => materialname;
            set
            {
                materialname = value;
                OnPropertyChanged();
            }
        }

        public string Unit
        {
            get => unit;
            set
            {
                unit = value;
                OnPropertyChanged();
            }
        }

        public bool Isdeleted
        {
            get => isdeleted; set
            {
                isdeleted = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<MaterialSupplierDTO> Materialsuppliers
        {
            get => _materialsuppliers;
            set
            {
                if (_materialsuppliers != value)
                {
                    _materialsuppliers = value;
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<ImportDetailDTO> Importdetails
        {
            get => _importdetails; set
            {
                if (_importdetails != value)
                {
                    _importdetails = value;
                    OnPropertyChanged();
                }
            }
        }

        public MaterialDTO Clone()
        {
            return new MaterialDTO()
            {
                Materialid = Materialid,
                Materialname = Materialname,
                Unit = Unit,
                Isdeleted = Isdeleted,
                Importdetails = Importdetails,
                Materialsuppliers = Materialsuppliers,
            };
        }
    }
}
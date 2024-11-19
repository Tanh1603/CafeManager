using CafeManager.Core.Data;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

#nullable disable

namespace CafeManager.Core.DTOs
{
    public class MaterialDTO : INotifyPropertyChanged
    {
        private int materialid;
        private string materialname;
        private string unit;
        private bool? isdeleted;

        private ObservableCollection<MaterialSupplierDTO> _materialsuppliersDTO = [];
        private ObservableCollection<ConsumedMaterialDTO> _consumedmaterialDTO = [];
        private ObservableCollection<ImportDetailDTO> _importdetailDTO = [];

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

        public bool? Isdeleted
        {
            get => isdeleted; set
            {
                isdeleted = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<MaterialSupplierDTO> MaterialsuppliersDTO
        {
            get => _materialsuppliersDTO;
            set
            {
                _materialsuppliersDTO = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<ConsumedMaterialDTO> ConsumedmaterialDTO
        {
            get => _consumedmaterialDTO;
            set
            {
                _consumedmaterialDTO = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<ImportDetailDTO> ImportdetailDTO
        {
            get => _importdetailDTO; set
            {
                _importdetailDTO = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public MaterialDTO Clone()
        {
            return new MaterialDTO()
            {
                Materialid = Materialid,
                Materialname = Materialname,
                Unit = Unit,
                Isdeleted = Isdeleted,

                ConsumedmaterialDTO = ConsumedmaterialDTO,
                ImportdetailDTO = ImportdetailDTO,
                MaterialsuppliersDTO = MaterialsuppliersDTO,
            };
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
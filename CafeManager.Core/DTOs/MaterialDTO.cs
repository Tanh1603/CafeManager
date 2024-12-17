using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

#nullable disable

namespace CafeManager.Core.DTOs
{
    public partial class MaterialDTO : BaseDTO
    {
        [ObservableProperty]
        private int materialid;

        [ObservableProperty]
        private string materialname;

        [ObservableProperty]
        private string unit;

        [ObservableProperty]
        private ObservableCollection<MaterialSupplierDTO> _materialsuppliers = [];

        public MaterialDTO Clone()
        {
            return new MaterialDTO()
            {
                Materialid = Materialid,
                Materialname = Materialname,
                Unit = Unit,
                Isdeleted = Isdeleted,
                Materialsuppliers = Materialsuppliers,
            };
        }
    }
}
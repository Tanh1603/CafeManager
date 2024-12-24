using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace CafeManager.Core.DTOs
{
    public partial class MaterialDTO : BaseDTO
    {
        [ObservableProperty]
        private int materialid;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessage = "Không được để trống")]
        private string materialname;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessage = "Không được để trống")]
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
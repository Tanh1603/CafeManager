using CafeManager.Core.Data;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

#nullable disable

namespace CafeManager.Core.DTOs
{
    public partial class MaterialSupplierDTO : BaseDTO
    {
        [ObservableProperty]
        private int _materialsupplierid;

        [ObservableProperty]
        private int materialid;

        [ObservableProperty]
        private int supplierid;

        [ObservableProperty]
        private DateTime _manufacturedate = DateTime.Now;

        [ObservableProperty]
        private DateTime _expirationdate = DateTime.Now;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessage = "Nguồn gốc không được trống")]
        private string _original;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessage = "Nhà sản xuất không được trống")]
        private string _manufacturer;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessage = "Giá không được trống")]
        private decimal _price;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessage = "Vật tư không được trống")]
        [NotifyPropertyChangedFor(nameof(TotalQuantity))]
        private MaterialDTO _material = new();

        [ObservableProperty]
        private SupplierDTO _supplier;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(TotalQuantity))]
        private ObservableCollection<ConsumedMaterialDTO> _consumedmaterials = [];

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(TotalQuantity))]
        private ObservableCollection<ImportDetailDTO> _importdetails = [];

        public decimal TotalQuantity => (Importdetails?.Where(x => x.Isdeleted == false).Sum(x => x.Quantity) ?? 0) - (Consumedmaterials?.Where(x => x.Isdeleted == false).Sum(x => x.Quantity) ?? 0);

        public MaterialSupplierDTO Clone()
        {
            return new MaterialSupplierDTO()
            {
                Id = Id,
                Materialsupplierid = Materialsupplierid,
                Materialid = Materialid,
                Supplierid = Supplierid,
                Manufacturedate = Manufacturedate,
                Expirationdate = Expirationdate,
                Original = Original,
                Manufacturer = Manufacturer,
                Price = Price,
                Isdeleted = Isdeleted,

                Material = Material,
                Supplier = Supplier,
                Importdetails = Importdetails,
                Consumedmaterials = Consumedmaterials,
            };
        }
    }
}
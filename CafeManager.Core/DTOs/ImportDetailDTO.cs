using CafeManager.Core.Data;
using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

#nullable disable

namespace CafeManager.Core.DTOs
{
    public partial class ImportDetailDTO : BaseDTO
    {
        [ObservableProperty]
        private int _importdetailid;

        [ObservableProperty]
        private int _importid;

        [ObservableProperty]
        private int _materialsupplierid;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessage = "Không được trống")]
        private MaterialSupplierDTO _materialsupplier = new();

        [ObservableProperty]
        private ImportDTO _import;

        private decimal _quantity;

        public decimal Quantity
        {
            get => _quantity;
            set
            {
                if (_quantity != value)
                {
                    _quantity = value;
                    OnPropertyChanged();
                    UpdateTotalPriceAction?.Invoke();
                }
            }
        }

        public ImportDetailDTO Clone()
        {
            return new ImportDetailDTO
            {
                Id = Id,
                Importdetailid = Importdetailid,
                Importid = Importid,
                Materialsupplierid = Materialsupplierid,
                Isdeleted = Isdeleted,
                Quantity = Quantity,

                Import = Import,
                Materialsupplier = Materialsupplier,
            };
        }

        public static Action UpdateTotalPriceAction { get; set; }
    }
}
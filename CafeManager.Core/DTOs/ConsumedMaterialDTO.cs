using CafeManager.Core.Data;
using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

#nullable disable

namespace CafeManager.Core.DTOs
{
    public partial class ConsumedMaterialDTO : BaseDTO
    {
        [ObservableProperty]
        private int _consumedmaterialid;

        [ObservableProperty]
        private int _materialsupplierid;

        [ObservableProperty]
        private decimal _quantity;

        [ObservableProperty]
        private MaterialSupplierDTO _materialsupplier;

        [ObservableProperty]
        private DateOnly _usagedate;

        public ConsumedMaterialDTO Clone()
        {
            return new ConsumedMaterialDTO
            {
                Consumedmaterialid = Consumedmaterialid,
                Materialsupplierid = Materialsupplierid,
                Quantity = Quantity,
                Isdeleted = Isdeleted,
                Materialsupplier = Materialsupplier,
                Usagedate = Usagedate
            };
        }
    }
}
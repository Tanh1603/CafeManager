using CafeManager.Core.Data;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

#nullable disable

namespace CafeManager.Core.DTOs
{
    public partial class SupplierDTO : BaseDTO
    {
        [ObservableProperty]
        private int _supplierid;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessage = "Không được trống")]
        private string _suppliername;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessage = "Không được trống")]
        private string _representativesupplier;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessage = "Không được trống")]
        private string _phone;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessage = "Không được trống")]
        private string _email;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessage = "Không được trống")]
        private string _address;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessage = "Không được trống")]
        private string _notes;

        [ObservableProperty]
        private ObservableCollection<ImportDTO> _imports = [];

        [ObservableProperty]
        private ObservableCollection<MaterialSupplierDTO> _materialsuppliers = [];

        public SupplierDTO Clone()
        {
            return new SupplierDTO
            {
                Id = Id,
                Supplierid = Supplierid,
                Suppliername = Suppliername,
                Representativesupplier = Representativesupplier,
                Phone = Phone,
                Email = Email,
                Address = Address,
                Notes = Notes,
                Isdeleted = Isdeleted,

                Imports = Imports,
                Materialsuppliers = Materialsuppliers
            };
        }
    }
}
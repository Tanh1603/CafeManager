using AutoMapper;
using CafeManager.Core.Data;
using CafeManager.Core.DTOs;
using CafeManager.Core.Services;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Import <-> ImportDTO
        CreateMap<Import, ImportDTO>();
        CreateMap<ImportDTO, Import>()
            .ForMember(dest => dest.Staff, opt => opt.Ignore())
            .ForMember(dest => dest.Supplier, opt => opt.Ignore())
            .ForMember(dest => dest.Importdetails, opt => opt.Ignore());

        // ImportDetail <-> ImportDetailDTO
        CreateMap<Importdetail, ImportDetailDTO>();
        CreateMap<ImportDetailDTO, Importdetail>()
            .ForMember(dest => dest.Material, opt => opt.Ignore())
            .ForMember(dest => dest.Import, opt => opt.Ignore());

        // Material <-> MaterialDTO
        CreateMap<Material, MaterialDTO>();
        CreateMap<MaterialDTO, Material>()
            .ForMember(dest => dest.Materialsuppliers, opt => opt.Ignore());

        // MaterialSupplier <-> MaterialSupplierDTO
        CreateMap<Materialsupplier, MaterialSupplierDTO>()
            .ForMember(dest => dest.TotalQuantity, opt => opt.MapFrom(src =>
                (src.Material.Importdetails != null
                    ? src.Material.Importdetails
                        .Where(id => id.Import != null && id.Import.Supplierid == src.Supplier.Supplierid)
                        .Sum(id => id.Quantity ?? 0)
                    : 0)
                -
                (src.Consumedmaterials != null
                    ? src.Consumedmaterials
                        .Where(cm => cm.Materialsupplierid == src.Materialsupplierid)
                        .Sum(cm => cm.Quantity ?? 0)
                    : 0)
            ));
        CreateMap<MaterialSupplierDTO, Materialsupplier>()
            .ForMember(dest => dest.Material, opt => opt.Ignore())
            .ForMember(dest => dest.Supplier, opt => opt.Ignore())
            .ForMember(dest => dest.Consumedmaterials, opt => opt.Ignore());

        // Supplier <-> SupplierDTO
        CreateMap<Supplier, SupplierDTO>();
        CreateMap<SupplierDTO, Supplier>()
            .ForMember(dest => dest.Imports, opt => opt.Ignore())
            .ForMember(dest => dest.Materialsuppliers, opt => opt.Ignore());

        // ConsumedMaterial <-> ConsumedMaterialDTO
        CreateMap<Consumedmaterial, ConsumedMaterialDTO>();
        CreateMap<ConsumedMaterialDTO, Consumedmaterial>()
            .ForMember(dest => dest.Materialsupplier, opt => opt.Ignore());

        // AppUserDTO <-> Appuser
        CreateMap<Appuser, AppUserDTO>()
            .ForMember(dest => dest.Avatar, opt => opt.MapFrom(src => ConvertImageServices.Base64ToBitmapImage(src.Avatar)));
        CreateMap<AppUserDTO, Appuser>()
            .ForMember(dest => dest.Avatar, opt => opt.MapFrom(src => ConvertImageServices.BitmapImageToBase64(src.Avatar)));

        // CoffeetableDTO <-> Coffeetable
        CreateMap<Coffeetable, CoffeetableDTO>();
        CreateMap<CoffeetableDTO, Coffeetable>();

        // FoodCategoryDTO <-> Foodcategory
        CreateMap<Foodcategory, FoodCategoryDTO>();
        CreateMap<FoodCategoryDTO, Foodcategory>()
            .ForMember(dest => dest.Foods, src => src.Ignore());

        // Food <-> FoodDTO
        CreateMap<Food, FoodDTO>()
            .ForMember(dest => dest.Imagefood, opt => opt.MapFrom(src => ConvertImageServices.Base64ToBitmapImage(src.Imagefood)));
        CreateMap<FoodDTO, Food>()
            .ForMember(dest => dest.Imagefood, opt => opt.MapFrom(src => ConvertImageServices.BitmapImageToBase64(src.Imagefood)))
            .ForMember(dest => dest.Foodcategory, opt => opt.Ignore());

        // Invoice <-> InvoiceDTO
        CreateMap<Invoice, InvoiceDTO>();
        CreateMap<InvoiceDTO, Invoice>()
            .ForMember(dest => dest.Invoicedetails, opt => opt.Ignore())
            .ForMember(dest => dest.Staff, opt => opt.Ignore())
            .ForMember(dest => dest.Coffeetable, opt => opt.Ignore());

        // Invoicedetail <-> InvoiceDetailDTO
        CreateMap<Invoicedetail, InvoiceDetailDTO>();
        CreateMap<InvoiceDetailDTO, Invoicedetail>()
            .ForMember(dest => dest.Food, opt => opt.Ignore());

        // Staff <-> StaffDTO
        CreateMap<Staff, StaffDTO>();
        CreateMap<StaffDTO, Staff>()
            .ForMember(dest => dest.Staffsalaryhistories, opt => opt.Ignore());

        // Staffsalaryhistories <-> StaffsalaryhistoriesDTO
        CreateMap<Staffsalaryhistory, StaffsalaryhistoryDTO>();
        CreateMap<StaffsalaryhistoryDTO, Staffsalaryhistory>();
    }
}
using AutoMapper;
using CafeManager.Core.Data;
using CafeManager.Core.DTOs;
using CafeManager.Core.Services;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Import <-> ImportDTO
        CreateMap<Import, ImportDTO>().ReverseMap();

        // ImportDetail <-> ImportDetailDTO
        CreateMap<Importdetail, ImportDetailDTO>().ReverseMap();

        // Material <-> MaterialDTO
        CreateMap<Material, MaterialDTO>().ReverseMap();

        // MaterialSupplier <-> MaterialSupplierDTO
        CreateMap<Materialsupplier, MaterialSupplierDTO>().ReverseMap();

        // Supplier <-> SupplierDTO
        CreateMap<Supplier, SupplierDTO>().ReverseMap();

        // ConsumedMaterial <-> ConsumedMaterialDTO
        CreateMap<Consumedmaterial, ConsumedMaterialDTO>().ReverseMap();

        // AppUserDTO <-> Appuser
        CreateMap<Appuser, AppUserDTO>()
            .ForMember(dest => dest.Avatar, opt => opt.MapFrom(src => ConvertImageServices.Base64ToBitmapImage(src.Avatar)))
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role == 1 ? "Admin" : "User"));
        CreateMap<AppUserDTO, Appuser>()
            .ForMember(dest => dest.Avatar, opt => opt.MapFrom(src => ConvertImageServices.BitmapImageToBase64(src.Avatar)))
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role == "Admin"));

        // CoffeetableDTO <-> Coffeetable
        CreateMap<Coffeetable, CoffeetableDTO>().ReverseMap();

        // FoodCategoryDTO <-> Foodcategory
        CreateMap<Foodcategory, FoodCategoryDTO>().ReverseMap();

        // Food <-> FoodDTO
        CreateMap<Food, FoodDTO>()
            .ForMember(dest => dest.Imagefood, opt => opt.MapFrom(src => ConvertImageServices.Base64ToBitmapImage(src.Imagefood)));
        CreateMap<FoodDTO, Food>()
            .ForMember(dest => dest.Imagefood, opt => opt.MapFrom(src => ConvertImageServices.BitmapImageToBase64(src.Imagefood)));

        // Invoice <-> InvoiceDTO
        CreateMap<Invoice, InvoiceDTO>();
        CreateMap<InvoiceDTO, Invoice>()
            .ForMember(dest => dest.Coffeetable, opt => opt.Ignore())
            .ForMember(dest => dest.Staff, opt => opt.Ignore());

        // Invoicedetail <-> InvoiceDetailDTO
        CreateMap<Invoicedetail, InvoiceDetailDTO>();
        CreateMap<InvoiceDetailDTO, Invoicedetail>()
            .ForMember(dest => dest.Food, opt => opt.Ignore());

        // Staff <-> StaffDTO
        CreateMap<Staff, StaffDTO>().ReverseMap();

        // Staffsalaryhistories <-> StaffsalaryhistoriesDTO
        CreateMap<Staffsalaryhistory, StaffsalaryhistoryDTO>().ReverseMap();
    }
}
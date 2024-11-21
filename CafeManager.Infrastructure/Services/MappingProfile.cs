using AutoMapper;
using CafeManager.Core.Data;
using CafeManager.Core.DTOs;

public class MappingProfile : Profile
{
    //public MappingProfile()
    //{
    //    // Import -> ImportDTO
    //    CreateMap<Import, ImportDTO>()
    //        .ForMember(dest => dest.StaffDTO, opt => opt.MapFrom(src => src.Staff))
    //        .ForMember(dest => dest.SupplierDTO, opt => opt.MapFrom(src => src.Supplier))
    //        .ForMember(dest => dest.ListImportDetailDTO, opt => opt.MapFrom(src => src.Importdetails)).PreserveReferences();
    //    CreateMap<ImportDTO, Import>()
    //        .ForMember(dest => dest.Staff, opt => opt.Ignore())
    //        .ForMember(dest => dest.Supplier, opt => opt.Ignore())
    //        .ForMember(dest => dest.Importdetails, opt => opt.Ignore());

    //    // ImportDetail -> ImportDetailDTO
    //    CreateMap<Importdetail, ImportDetailDTO>()
    //        .ForMember(dest => dest.MaterialDTO, opt => opt.MapFrom(src => src.Material))
    //        .ForMember(dest => dest.ImportDTO, opt => opt.MapFrom(src => src.Import)).PreserveReferences();
    //    CreateMap<ImportDetailDTO, Importdetail>()
    //        .ForMember(dest => dest.Material, opt => opt.Ignore())
    //        .ForMember(dest => dest.Import, opt => opt.Ignore());

    //    // Material -> MaterialDTO
    //    CreateMap<Material, MaterialDTO>()
    //        .ForMember(dest => dest.MaterialsuppliersDTO, opt => opt.MapFrom(src => src.Materialsuppliers)).PreserveReferences();
    //    CreateMap<MaterialDTO, Material>()
    //        .ForMember(dest => dest.Materialsuppliers, opt => opt.Ignore());

    //    // MaterialSupplier -> MaterialSupplierDTO
    //    CreateMap<Materialsupplier, MaterialSupplierDTO>()
    //        .ForMember(dest => dest.MaterialDTO, opt => opt.MapFrom(src => src.Material))
    //        .ForMember(dest => dest.SupplierDTO, opt => opt.MapFrom(src => src.Supplier))
    //        .ForMember(dest => dest.ConsumedMaterialDTO, opt => opt.MapFrom(src => src.Consumedmaterials))
    //        .ForMember(dest => dest.TotalQuantity, opt => opt.MapFrom(src =>
    //            (src.Material.Importdetails != null
    //                ? src.Material.Importdetails
    //                    .Where(id => id.Import != null && id.Import.Supplierid == src.Supplier.Supplierid)
    //                    .Sum(id => id.Quantity ?? 0)
    //                : 0)
    //            -
    //            (src.Consumedmaterials != null
    //                ? src.Consumedmaterials
    //                    .Where(cm => cm.Materialsupplierid == src.Materialsupplierid)
    //                    .Sum(cm => cm.Quantity ?? 0)
    //                : 0)
    //        )).PreserveReferences();
    //    CreateMap<MaterialSupplierDTO, Materialsupplier>()
    //        .ForMember(dest => dest.Material, opt => opt.Ignore())
    //        .ForMember(dest => dest.Supplier, opt => opt.Ignore())
    //        .ForMember(dest => dest.Consumedmaterials, opt => opt.Ignore());

    //    // Supplier -> SupplierDTO
    //    CreateMap<Supplier, SupplierDTO>()
    //        .ForMember(dest => dest.ImportDTO, opt => opt.MapFrom(x => x.Imports))
    //        .ForMember(dest => dest.MaterialsupplierDTO, opt => opt.MapFrom(x => x.Materialsuppliers))
    //        .PreserveReferences();

    //    CreateMap<SupplierDTO, Supplier>()
    //        .ForMember(dest => dest.Imports, opt => opt.Ignore())
    //        .ForMember(dest => dest.Materialsuppliers, opt => opt.Ignore()).PreserveReferences();

    //    // ConsumedMaterial -> ConsumedMaterialDTO
    //    CreateMap<Consumedmaterial, ConsumedMaterialDTO>()
    //        .ForMember(dest => dest.MaterialSupplierDTO, opt => opt.MapFrom(src => src.Materialsupplier)).PreserveReferences();
    //    CreateMap<ConsumedMaterialDTO, Consumedmaterial>()
    //        .ForMember(dest => dest.Materialsupplier, opt => opt.Ignore());
    //}

    public MappingProfile()
    {
        // Import <-> ImportDTO
        CreateMap<Import, ImportDTO>()
            .ForMember(dest => dest.StaffDTO, opt => opt.MapFrom(src => src.Staff))
            .ForMember(dest => dest.SupplierDTO, opt => opt.MapFrom(src => src.Supplier))
            .ForMember(dest => dest.ListImportDetailDTO, opt => opt.MapFrom(src => src.Importdetails));
        CreateMap<ImportDTO, Import>()
            .ForMember(dest => dest.Staff, opt => opt.Ignore())
            .ForMember(dest => dest.Supplier, opt => opt.Ignore())
            .ForMember(dest => dest.Importdetails, opt => opt.Ignore());

        // ImportDetail <-> ImportDetailDTO
        CreateMap<Importdetail, ImportDetailDTO>()
            .ForMember(dest => dest.MaterialDTO, opt => opt.MapFrom(src => src.Material))
            .ForMember(dest => dest.ImportDTO, opt => opt.Ignore());

        CreateMap<ImportDetailDTO, Importdetail>()
            .ForMember(dest => dest.Material, opt => opt.Ignore())
            .ForMember(dest => dest.Import, opt => opt.Ignore());

        // Material <-> MaterialDTO
        CreateMap<Material, MaterialDTO>()
            .ForMember(dest => dest.MaterialsuppliersDTO, opt => opt.MapFrom(src => src.Materialsuppliers));
        CreateMap<MaterialDTO, Material>()
            .ForMember(dest => dest.Materialsuppliers, opt => opt.Ignore());

        // MaterialSupplier <-> MaterialSupplierDTO
        CreateMap<Materialsupplier, MaterialSupplierDTO>()
            .ForMember(dest => dest.MaterialDTO, opt => opt.MapFrom(src => src.Material))
            .ForMember(dest => dest.SupplierDTO, opt => opt.MapFrom(src => src.Supplier))
            .ForMember(dest => dest.ConsumedMaterialDTO, opt => opt.MapFrom(src => src.Consumedmaterials))
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
        CreateMap<Supplier, SupplierDTO>()
            .ForMember(dest => dest.ImportDTO, opt => opt.Ignore())
            .ForMember(dest => dest.MaterialsupplierDTO, opt => opt.MapFrom(x => x.Materialsuppliers));
        CreateMap<SupplierDTO, Supplier>()
            .ForMember(dest => dest.Imports, opt => opt.Ignore())
            .ForMember(dest => dest.Materialsuppliers, opt => opt.Ignore());

        // ConsumedMaterial <-> ConsumedMaterialDTO
        CreateMap<Consumedmaterial, ConsumedMaterialDTO>()
            .ForMember(dest => dest.MaterialSupplierDTO, opt => opt.MapFrom(src => src.Materialsupplier));
        CreateMap<ConsumedMaterialDTO, Consumedmaterial>()
            .ForMember(dest => dest.Materialsupplier, opt => opt.Ignore());
    }
}
using CafeManager.Core.Data;
using CafeManager.Core.DTOs;

namespace CafeManager.Core.Services
{
    public static class MaterialSupplierMapper
    {
        public static MaterialSupplierDTO ToDTO(this Materialsupplier entity, bool isLazyLoad = false, HashSet<object> visited = null)
        {
            if (entity == null) return null;

            visited ??= new HashSet<object>();
            if (visited.Contains(entity)) return null;
            visited.Add(entity);

            MaterialSupplierDTO dto = new MaterialSupplierDTO
            {
                Materialsupplierid = entity.Materialsupplierid,
                Materialid = entity.Materialid,
                Supplierid = entity.Supplierid,
                Manufacturedate = entity.Manufacturedate,
                Expirationdate = entity.Expirationdate,
                Original = entity.Original,
                Manufacturer = entity.Manufacturer,
                Price = entity.Price ?? 0,
                Isdeleted = entity.Isdeleted,
            };
            if (isLazyLoad)
            {
                dto.MaterialDTO = entity.Material.ToDTO(true, visited);
                dto.ConsumedMaterialDTO = [.. entity.Consumedmaterials.Select(x => x.ToDTO(true, visited))];
                dto.SupplierDTO = entity.Supplier.ToDTO(true, visited);
            }
            return dto;
        }

        public static Materialsupplier ToEntity(this MaterialSupplierDTO dto)
        {
            if (dto == null) return null;

            return new Materialsupplier
            {
                Materialsupplierid = dto.Materialsupplierid,
                Materialid = dto.Materialid,
                Supplierid = dto.Supplierid,
                Manufacturedate = dto.Manufacturedate,
                Expirationdate = dto.Expirationdate,
                Original = dto.Original,
                Manufacturer = dto.Manufacturer,
                Price = dto.Price,
                Isdeleted = dto.Isdeleted,
            };
        }
    }
}
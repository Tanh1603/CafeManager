using CafeManager.Core.Data;
using CafeManager.Core.DTOs;

namespace CafeManager.Core.Services
{
    public static class ConsumedMaterialMapper
    {
        public static ConsumedMaterialDTO ToDTO(this Consumedmaterial entity, bool isLazyLoad = false, HashSet<object> visited = null)
        {
            if (entity == null) return null;

            visited ??= new HashSet<object>();
            if (visited.Contains(entity)) return null;
            visited.Add(entity);

            ConsumedMaterialDTO dto = new ConsumedMaterialDTO
            {
                Consumedmaterialid = entity.Consumedmaterialid,
                Materialsupplierid = entity.Materialsupplierid,
                Quantity = entity.Quantity ?? 0,
                Isdeleted = entity.Isdeleted,
            };
            if (isLazyLoad)
            {
                dto.MaterialSupplierDTO = entity.Materialsupplier.ToDTO(true, visited);
            }
            return dto;
        }

        public static Consumedmaterial ToEntity(this ConsumedMaterialDTO dto)
        {
            if (dto == null) return null;

            return new Consumedmaterial
            {
                Consumedmaterialid = dto.Consumedmaterialid,
                Materialsupplierid = dto.Materialsupplierid,
                Quantity = dto.Quantity,
                Isdeleted = dto.Isdeleted,
            };
        }
    }
}
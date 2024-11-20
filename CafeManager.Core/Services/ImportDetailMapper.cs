using CafeManager.Core.Data;
using CafeManager.Core.DTOs;

namespace CafeManager.Core.Services
{
    public static class ImportDetailMapper
    {
        public static ImportDetailDTO ToDTO(this Importdetail entity, bool isLazyLoad = false, HashSet<object> visited = null)
        {
            if (entity == null) return null;

            visited ??= new HashSet<object>();
            if (visited.Contains(entity)) return null;
            visited.Add(entity);

            var dto = new ImportDetailDTO
            {
                Importdetailid = entity.Importdetailid,
                Importid = entity.Importid,
                Materialid = entity.Materialid,
                Quantity = entity.Quantity ?? 0,
                Isdeleted = entity.Isdeleted,
            };
            if (isLazyLoad)
            {
                dto.MaterialDTO = entity.Material.ToDTO(true, visited);
                dto.ImportDTO = entity.Import.ToDTO(true, visited);
            }
            return dto;
        }

        public static Importdetail ToEntity(this ImportDetailDTO dto)
        {
            if (dto == null) return null;
            return new Importdetail
            {
                Importdetailid = dto.Importdetailid,
                Importid = dto.Importid,
                Materialid = dto.Materialid,
                Quantity = dto.Quantity,
                Isdeleted = dto.Isdeleted,
            };
        }
    }
}
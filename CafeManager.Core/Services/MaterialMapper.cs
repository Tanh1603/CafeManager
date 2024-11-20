using CafeManager.Core.Data;
using CafeManager.Core.DTOs;

namespace CafeManager.Core.Services
{
    public static class MaterialMapper
    {
        public static MaterialDTO ToDTO(this Material entity, bool isLazyLoad = false, HashSet<object> visited = null)
        {
            if (entity == null) return null;

            visited ??= new HashSet<object>();
            if (visited.Contains(entity)) return null;
            visited.Add(entity);

            MaterialDTO dto = new MaterialDTO
            {
                Materialid = entity.Materialid,
                Materialname = entity.Materialname,
                Unit = entity.Unit,
                Isdeleted = entity.Isdeleted,
            };
            if (isLazyLoad)
            {
                dto.MaterialsuppliersDTO = [.. entity.Materialsuppliers.Select(x => x.ToDTO(true, visited))];
                dto.ImportdetailDTO = [.. entity.Importdetails.Select(x => x.ToDTO(true, visited))];
            }
            return dto;
        }

        public static Material ToEntity(this MaterialDTO dto)
        {
            if (dto == null) return null;
            return new Material
            {
                Materialid = dto.Materialid,
                Materialname = dto.Materialname,
                Unit = dto.Unit,
                Isdeleted = dto.Isdeleted,
            };
        }
    }
}
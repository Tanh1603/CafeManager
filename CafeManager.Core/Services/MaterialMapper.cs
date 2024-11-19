using CafeManager.Core.Data;
using CafeManager.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeManager.Core.Services
{
    public static class MaterialMapper
    {
        public static MaterialDTO ToDTO(Material entity)
        {
            if (entity == null) return null;

            return new MaterialDTO
            {
                Materialid = entity.Materialid,
                Materialname = entity.Materialname,
                Unit = entity.Unit,
                Isdeleted = entity.Isdeleted,

                MaterialsuppliersDTO = [.. entity.Materialsuppliers.Select(x => MaterialSupplierMapper.ToDTO(x))]
            };
        }

        public static Material ToEntity(MaterialDTO dto)
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
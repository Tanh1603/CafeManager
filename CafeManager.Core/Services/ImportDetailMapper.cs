
using CafeManager.Core.Data;
using CafeManager.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CafeManager.Core.Services
{
    public class ImportDetailMapper
    {
        public static ImportDetailDTO ToDTO(Importdetail entity)
        {
            if (entity == null) return null;

            return new ImportDetailDTO
            {
                Importdetailid = entity.Importdetailid,
                ImportId = entity.Importid,
                MaterialId = entity.Materialid,
                Quantity = entity.Quantity,
                ModifyMaterialDetail = new MaterialDetailDTO()
                {
                    CurrentMaterial = new Material()
                },
                Isdeleted = entity.Isdeleted,
            };
        }

        public static Importdetail ToEntity(ImportDetailDTO dto)
        {
            if (dto == null) return null;
            return new Importdetail
            {
                Importdetailid = dto.Importdetailid,
                Importid = dto.ImportId,
                Materialid = dto.MaterialId,
                Quantity = dto.Quantity,
                Isdeleted = dto.Isdeleted
            };
        }
    }
}

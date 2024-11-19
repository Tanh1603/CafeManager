﻿using CafeManager.Core.Data;
using CafeManager.Core.DTOs;

namespace CafeManager.Core.Services
{
    public static class ImportDetailMapper
    {
        public static ImportDetailDTO ToDTO(Importdetail entity)
        {
            if (entity == null) return null;

            return new ImportDetailDTO
            {
                Importdetailid = entity.Importdetailid,
                Importid = entity.Importid,
                Materialid = entity.Materialid,
                Quantity = entity.Quantity ?? 0,
                Isdeleted = entity.Isdeleted,

                MaterialDTO = MaterialMapper.ToDTO(entity.Material)
            };
        }

        public static Importdetail ToEntity(ImportDetailDTO dto)
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
using CafeManager.Core.Data;
using CafeManager.Core.DTOs;

namespace CafeManager.Core.Services
{
    public static class ConsumedMaterialMapper
    {
        public static ConsumedMaterialDTO ToDTO(Consumedmaterial entity)
        {
            if (entity == null) return null;

            return new ConsumedMaterialDTO
            {
                Consumedmaterialid = entity.Consumedmaterialid,
                Materialid = entity.Materialid,
                Quantity = entity.Quantity ?? 0,
                Isdeleted = entity.Isdeleted,

                //MaterialDTO = MaterialMapper.ToDTO(entity.Material),
            };
        }

        public static Consumedmaterial ToEntity(ConsumedMaterialDTO dto)
        {
            if (dto == null) return null;

            return new Consumedmaterial
            {
                Consumedmaterialid = dto.Consumedmaterialid,
                Materialid = dto.Materialid,
                Quantity = dto.Quantity,
                Isdeleted = dto.Isdeleted,
            };
        }
    }
}
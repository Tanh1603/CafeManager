using CafeManager.Core.Data;
using CafeManager.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace CafeManager.Core.Services
{
    public class MaterialDetailMapper
    {
        public static MaterialDetailDTO ToDTO(Materialsupplier entity, decimal? quantity)
        {
            if (entity == null) return null;

            return new MaterialDetailDTO
            {
                Materialsupplierid = entity.Materialsupplierid,
                CurrentMaterial = entity.Material,
                Price = entity.Price,
                Original = entity.Original,
                Manufacturer = entity.Manufacturer,
                Manufacturedate = entity.Manufacturedate,
                Expirationdate = entity.Expirationdate,
                Quantity = quantity,
                Isdeleted = entity.Isdeleted
            };
        }

        public static Materialsupplier ToEntity(MaterialDetailDTO dto, int supplierid)
        {
            if (dto == null) return null;

            return new Materialsupplier
            {
                Materialsupplierid = dto.Materialsupplierid,
                Materialid = dto.CurrentMaterial.Materialid,
                Supplierid = supplierid,
                Price = dto.Price,
                Original = dto.Original,
                Manufacturer = dto.Manufacturer,
                Manufacturedate = dto.Manufacturedate,
                Expirationdate = dto.Expirationdate,
                Isdeleted = dto.Isdeleted
            };
        }

    }
}

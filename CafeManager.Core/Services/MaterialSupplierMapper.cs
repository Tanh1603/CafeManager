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
    public class MaterialSupplierMapper
    {
        public static MaterialSupplierDTO ToDTO(Materialsupplier entity)
        {
            if (entity == null) return null;

            return new MaterialSupplierDTO
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

                MaterialDTO = MaterialMapper.ToDTO(entity.Material),
                SupplierDTO = SupplierMapper.ToDTO(entity.Supplier),
            };
        }

        public static Materialsupplier ToEntity(MaterialSupplierDTO dto)
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
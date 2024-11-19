using CafeManager.Core.Data;
using CafeManager.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeManager.Core.Services
{
    public static class SupplierMapper
    {
        public static SupplierDTO ToDTO(Supplier entity)
        {
            if (entity == null) return null;

            return new SupplierDTO
            {
                Supplierid = entity.Supplierid,
                Suppliername = entity.Suppliername,
                Representativesupplier = entity.Representativesupplier,
                Phone = entity.Phone,
                Email = entity.Email,
                Address = entity.Address,
                Notes = entity.Notes,
                Isdeleted = entity.Isdeleted,
            };
        }

        public static Supplier ToEntity(SupplierDTO dto)
        {
            if (dto == null) return null;

            return new Supplier
            {
                Supplierid = dto.Supplierid,
                Suppliername = dto.Suppliername,
                Representativesupplier = dto.Representativesupplier,
                Phone = dto.Phone,
                Email = dto.Email,
                Address = dto.Address,
                Notes = dto.Notes,
                Isdeleted = dto.Isdeleted,
            };
        }
    }
}
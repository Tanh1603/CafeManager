﻿using CafeManager.Core.Data;
using CafeManager.Core.DTOs;
using CafeManager.Core.Repositories;
using CafeManager.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeManager.Infrastructure.Repositories
{
    public class MaterialRepository : Repository<Material>, IMaterialRepository
    {
        public MaterialRepository(CafeManagerContext cafeManagerContext) : base(cafeManagerContext)
        {
        }

        public async Task<IEnumerable<Material>> GetAllMaterialAsync()
        {
            return await _cafeManagerContext.Materials.Include(x => x.Consumedmaterials).ToListAsync();
        }

        public async Task<IEnumerable<MaterialDetailDTO>> GetAllMaterialWithDetail()
        {
            var result = await (
            from m in _cafeManagerContext.Materials
            join ms in _cafeManagerContext.Materialsuppliers on m.Materialid equals ms.Materialid
            join s in _cafeManagerContext.Suppliers on ms.Supplierid equals s.Supplierid
            join imd in _cafeManagerContext.Importdetails on ms.Materialsupplierid equals imd.Materialsupplierid
            where m.Isdeleted == false && ms.Isdeleted == false && imd.Isdeleted == false && s.Isdeleted == false
            group imd by new
            {
                m.Materialname,
                m.Unit,
                s.Suppliername,
                ms.Manufacturer,
                ms.Original,
                ms.Manufacturedate,
                ms.Expirationdate,
                ms.Price
            } into g
            select new MaterialDetailDTO
            {
                Materialname = g.Key.Materialname,
                Unit = g.Key.Unit,
                Manufacturer = g.Key.Manufacturer,
                Original = g.Key.Original,
                Manufacturedate = g.Key.Manufacturedate,
                Expirationdate = g.Key.Expirationdate,
                Suppliername = g.Key.Suppliername,
                Price = g.Key.Price ?? 0,

                Quantity = g.Sum(x => x.Quantity ?? 0)
            }
            ).ToListAsync();
            return result;
        }

        public async Task<IEnumerable<MaterialDetailDTO>> GetAllUsedMaterial()
        {
            var result = await (
            from cs in _cafeManagerContext.Consumedmaterials
            join m in _cafeManagerContext.Materials on cs.Materialid equals m.Materialid
            join ms in _cafeManagerContext.Materialsuppliers on m.Materialid equals ms.Materialid
            join s in _cafeManagerContext.Suppliers on ms.Supplierid equals s.Supplierid
            join imd in _cafeManagerContext.Importdetails on ms.Materialsupplierid equals imd.Materialsupplierid
            where m.Isdeleted == false && ms.Isdeleted == false && imd.Isdeleted == false && s.Isdeleted == false && cs.Isdeleted == false
            group cs by new
            {
                m.Materialname,
                m.Unit,
                s.Suppliername,
                ms.Manufacturer,
                ms.Original,
                ms.Manufacturedate,
                ms.Expirationdate,
                ms.Price
            } into g
            select new MaterialDetailDTO
            {
                Materialname = g.Key.Materialname,
                Unit = g.Key.Unit,
                Manufacturer = g.Key.Manufacturer,
                Original = g.Key.Original,
                Manufacturedate = g.Key.Manufacturedate,
                Expirationdate = g.Key.Expirationdate,
                Suppliername = g.Key.Suppliername,
                Price = g.Key.Price ?? 0,

                Quantity = g.Sum(x => (int)(x.Quantity ?? 0))
            }
            ).ToListAsync();
            return result;
        }
    }
}
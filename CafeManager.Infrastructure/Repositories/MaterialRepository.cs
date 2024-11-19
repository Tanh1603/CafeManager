using CafeManager.Core.Data;
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
                join imd in _cafeManagerContext.Importdetails on m.Materialid equals imd.Materialid
                join imp in _cafeManagerContext.Imports on imd.Importid equals imp.Importid
                where
                    m.Isdeleted == false &&
                    ms.Isdeleted == false &&
                    imd.Isdeleted == false &&
                    s.Isdeleted == false &&
                    imp.Isdeleted == false
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
                    CurrentMaterial = new Material { Materialname = g.Key.Materialname, Unit = g.Key.Unit },
                    Manufacturer = g.Key.Manufacturer,
                    Original = g.Key.Original,
                    Manufacturedate = g.Key.Manufacturedate,
                    Expirationdate = g.Key.Expirationdate,
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
                join imd in _cafeManagerContext.Importdetails on ms.Materialsupplierid equals imd.Materialid
                where
                    m.Isdeleted == false &&
                    ms.Isdeleted == false &&
                    imd.Isdeleted == false &&
                    s.Isdeleted == false &&
                    cs.Isdeleted == false
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
                    CurrentMaterial = new Material { Materialname = g.Key.Materialname, Unit = g.Key.Unit },
                    Manufacturer = g.Key.Manufacturer,
                    Original = g.Key.Original,
                    Manufacturedate = g.Key.Manufacturedate,
                    Expirationdate = g.Key.Expirationdate,
                    Price = g.Key.Price ?? 0,

                    Quantity = g.Sum(x => (int)(x.Quantity ?? 0))
                }
            ).ToListAsync();

            return result;
        }

        public async Task<Material> GetMaterialById(int id)
        {
            return await _cafeManagerContext.Materials.Where(x => x.Isdeleted == false)
                .Include(m => m.Materialsuppliers)
                .ThenInclude(x => x.Supplier)
                .FirstOrDefaultAsync(x => x.Materialid == id);
        }

        public async Task<MaterialDetailDTO> GetMaterialsuppliersByImportDetail(int materialid, int supplierid, int importdetailid)
        {
            var result = await (
                from m in _cafeManagerContext.Materials
                join ms in _cafeManagerContext.Materialsuppliers on m.Materialid equals ms.Materialid
                join imd in _cafeManagerContext.Importdetails on ms.Materialsupplierid equals imd.Materialid
                where
                    m.Isdeleted == false &&
                    ms.Isdeleted == false &&
                    imd.Isdeleted == false &&
                    ms.Materialid == materialid &&
                    ms.Supplierid == supplierid &&
                    imd.Importdetailid == importdetailid
                group m by new
                {
                    ms.Materialsupplierid,
                    m.Materialname,
                    m.Unit,
                    imd.Quantity,
                    ms.Manufacturer,
                    ms.Original,
                    ms.Manufacturedate,
                    ms.Expirationdate,
                    ms.Price
                } into g
                select new MaterialDetailDTO
                {
                    Materialsupplierid = g.Key.Materialsupplierid,
                    CurrentMaterial = new Material { Materialname = g.Key.Materialname, Unit = g.Key.Unit },
                    Quantity = g.Key.Quantity,
                    Manufacturer = g.Key.Manufacturer,
                    Original = g.Key.Original,
                    Manufacturedate = g.Key.Manufacturedate,
                    Expirationdate = g.Key.Expirationdate,
                    Price = g.Key.Price ?? 0
                }
            ).FirstOrDefaultAsync();

            return result;
        }

    }
}
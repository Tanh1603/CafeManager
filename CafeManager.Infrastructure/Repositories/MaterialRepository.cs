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
            return await _cafeManagerContext.Materials
                .ToListAsync();
        }

        public async Task<Material?> GetMaterialById(int id)
        {
            return await _cafeManagerContext.Materials.Where(x => x.Isdeleted == false)
                .Include(m => m.Materialsuppliers)
                .ThenInclude(x => x.Supplier)
                .FirstOrDefaultAsync(x => x.Materialid == id);
        }
    }
}
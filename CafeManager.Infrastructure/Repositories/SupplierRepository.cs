﻿using CafeManager.Core.Data;
using CafeManager.Core.Repositories;
using CafeManager.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace CafeManager.Infrastructure.Repositories
{
    public class SupplierRepository : Repository<Supplier>, ISupplierRepository
    {
        public SupplierRepository(CafeManagerContext cafeManagerContext) : base(cafeManagerContext)
        {
        }

        public async Task<IEnumerable<Supplier>> GetAllSupplierAsync()
        {
            return await _cafeManagerContext.Set<Supplier>()
                        .Where(m => m.Isdeleted == false)
                        .Include(m => m.Materialsuppliers)
                        .ThenInclude(ms => ms.Material)
                        .ToListAsync();
        }
    }
}
using CafeManager.Core.Data;
using CafeManager.Core.Repositories;
using CafeManager.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace CafeManager.Infrastructure.Repositories
{
    public class MaterialSupplierRepository : Repository<Materialsupplier>, IMaterialSupplierRepository
    {
        public MaterialSupplierRepository(CafeManagerContext cafeManagerContext) : base(cafeManagerContext)
        {
        }
    }
}
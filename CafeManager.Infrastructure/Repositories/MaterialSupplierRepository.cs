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

        public async Task<int> GetToTalMaterialSupplier(CancellationToken token = default)
        {
            try
            {
                return await _cafeManagerContext.Materialsuppliers.Where(x => x.Isdeleted == false).CountAsync(token);
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
using CafeManager.Core.Data;

namespace CafeManager.Core.Repositories
{
    public interface IMaterialSupplierRepository : IRepository<Materialsupplier>
    {
        Task<IEnumerable<Materialsupplier>> GetAllMaterialSuppierAsync();
    }
}
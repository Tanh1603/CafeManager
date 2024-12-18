using CafeManager.Core.Data;

namespace CafeManager.Core.Repositories
{
    public interface IMaterialSupplierRepository : IRepository<Materialsupplier>
    {
        Task<int> GetToTalMaterialSupplier(CancellationToken token = default);
    }
}
using CafeManager.Core.Data;

namespace CafeManager.Core.Repositories
{
    public interface IConsumedMaterialRepository : IRepository<Consumedmaterial>
    {
        public Task<IEnumerable<Consumedmaterial>> GetAllConsumedMaterialsAsync();
    }
}
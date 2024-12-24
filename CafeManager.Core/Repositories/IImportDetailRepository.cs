using CafeManager.Core.Data;

namespace CafeManager.Core.Repositories
{
    public interface IImportDetailRepository : IRepository<Importdetail>
    {
        Task<IEnumerable<Importdetail>> GetAllImportDetailAsync();

        Task<Importdetail> GetImportDetailByIdAsync(int id);


    }
}
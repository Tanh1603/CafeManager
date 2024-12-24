using System.Linq.Expressions;

namespace CafeManager.Core.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task<T> Create(T entity, CancellationToken token = default);

        Task<T?> Update(T entity, CancellationToken token = default);

        Task<bool> Delete(int id, CancellationToken token = default);

        Task<IEnumerable<T>> AddArange(IEnumerable<T> entities, CancellationToken token = default);

        Task<IEnumerable<T>> GetAll(CancellationToken token = default);

        Task<IEnumerable<T>> GetAllExistedAsync(CancellationToken token = default);

        Task<IEnumerable<T>> GetAllDeletedAsync(CancellationToken token = default);

        Task<T?> GetById(int id, CancellationToken token = default);

        Task<(IEnumerable<T> Items, int TotalCount)> GetByPageAsync(int pageIndex, int pageSize, Expression<Func<T, bool>>? filter = null, CancellationToken token = default);
    }
}
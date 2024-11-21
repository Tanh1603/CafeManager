using System.Linq.Expressions;

namespace CafeManager.Core.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task<T> Create(T entity);

        T? Update(T entity);

        Task<bool> Delete(int id);

        Task<IEnumerable<T>> AddArange(IEnumerable<T> entities);

        Task<IEnumerable<T>> GetAll();

        Task<T?> GetById(int id);

        Task<(IEnumerable<T> Items, int TotalCount)> GetByPageAsync(int pageIndex, int pageSize, Expression<Func<T, bool>>? filter = null);
    }
}
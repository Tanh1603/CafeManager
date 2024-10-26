using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CafeManager.Core.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task<T> GetById(int id);

        Task<IEnumerable<T>> GetAllAsync();

        Task<T> Create(T entity);

        Task<T> Update(T entity);

        Task<bool> Delete(int id);

        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);

        Task<IEnumerable<T>> GetSortedAsync<TKey>(Expression<Func<T, TKey>> keySelector, bool ascending = true);

        Task<IEnumerable<T>> GetPagedAsync(int skip, int take);

        Task<IEnumerable<T>> SearchAndSortAsync(Expression<Func<T, bool>> searchPredicate, Expression<Func<T, object>> sortKeySelector, bool ascending, int skip, int take);
    }
}
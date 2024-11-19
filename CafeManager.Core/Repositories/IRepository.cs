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
        Task<T> Create(T entity);

        T? Update(T? entity);

        Task<bool> Delete(int id);

        Task<IEnumerable<T>> AddArange(IEnumerable<T> entities);

        Task<IEnumerable<T>> GetAll();

        Task<T?> GetById(int id);
    }
}
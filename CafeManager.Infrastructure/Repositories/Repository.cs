using CafeManager.Core.Repositories;
using CafeManager.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CafeManager.Infrastructure.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected CafeManagerContext _cafeManagerContext;

        public Repository(IServiceProvider provider)
        {
            _cafeManagerContext = provider.GetRequiredService<CafeManagerContextFactory>().CreateDbContext();
        }

        public async Task<T> Create(T entity)
        {
            EntityEntry<T> entityEntry = await _cafeManagerContext.Set<T>().AddAsync(entity);
            return entityEntry.Entity;
        }

        public async Task<bool> Delete(int id)
        {
            var entity = await _cafeManagerContext.Set<T>().FindAsync(id);
            if (entity != null)
            {
                var property = typeof(T).GetProperty("Isdeleted");
                if (property != null)
                {
                    property.SetValue(entity, true);
                    return true;
                }
            }
            return false;
        }

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _cafeManagerContext.Set<T>().Where(predicate).ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _cafeManagerContext.Set<T>().ToListAsync();
        }

        public async Task<T> GetById(int id)
        {
            return await _cafeManagerContext.Set<T>().FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetPagedAsync(int skip, int take)
        {
            return await _cafeManagerContext.Set<T>().Skip(skip).Take(take).ToListAsync();
        }

        public async Task<IEnumerable<T>> GetSortedAsync<TKey>(Expression<Func<T, TKey>> keySelector, bool ascending = true)
        {
            var query = _cafeManagerContext.Set<T>().AsQueryable();

            query = ascending ? query.OrderBy(keySelector) : query.OrderByDescending(keySelector);

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<T>> SearchAndSortAsync(Expression<Func<T, bool>>? searchPredicate, Expression<Func<T, object>>? sortKeySelector, bool ascending, int skip, int take)
        {
            var query = _cafeManagerContext.Set<T>().Where(searchPredicate);
            query = ascending ? query.OrderBy(sortKeySelector) : query.OrderByDescending(sortKeySelector);
            return await query.Skip(skip).Take(take).ToListAsync();
        }

        public async Task<T> Update(T entity)
        {
            _cafeManagerContext.Set<T>().Update(entity);

            return entity;
        }
    }
}
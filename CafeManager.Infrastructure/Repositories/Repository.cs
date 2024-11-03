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

        public Repository(CafeManagerContext cafeManagerContext)
        {
            _cafeManagerContext = cafeManagerContext;
        }

        public virtual async Task<T> Create(T entity)
        {
            EntityEntry<T> entityEntry = await _cafeManagerContext.Set<T>().AddAsync(entity);
            return entityEntry.Entity;
        }

        public virtual T? Update(T? entity)
        {
            var existingEntity = _cafeManagerContext.Set<T>().Local.FirstOrDefault(e => e == entity);

            if (existingEntity != null)
            {
                _cafeManagerContext.Entry(existingEntity).CurrentValues.SetValues(entity);
            }
            else
            {
                _cafeManagerContext.Set<T>().Attach(entity);
                _cafeManagerContext.Entry(entity).State = EntityState.Modified;
            }

            return entity;
        }

        public virtual async Task<bool> Delete(int id)
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

        private bool IsEntityNotDeleted(T entity)
        {
            var property = typeof(T).GetProperty("Isdeleted");
            if (property != null)
            {
                var isDeletedValue = property.GetValue(entity) as bool?;
                return isDeletedValue != true;
            }
            return true;
        }

        public async Task<IEnumerable<T>> AddArange(IEnumerable<T> entities)
        {
            if (entities == null || !entities.Any())
                return Enumerable.Empty<T>();

            await _cafeManagerContext.AddRangeAsync(entities);
            return entities;
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await _cafeManagerContext.Set<T>().ToListAsync();
        }
    }
}
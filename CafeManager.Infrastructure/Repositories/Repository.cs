using CafeManager.Core.Repositories;
using CafeManager.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;

namespace CafeManager.Infrastructure.Repositories
{
    public class Repository<T>(CafeManagerContext cafeManagerContext) : IRepository<T> where T : class
    {
        protected CafeManagerContext _cafeManagerContext
            = cafeManagerContext;

        public virtual async Task<T> Create(T entity)
        {
            EntityEntry<T> entityEntry = await _cafeManagerContext.Set<T>().AddAsync(entity);
            return entityEntry.Entity;
        }

        public virtual async Task<T?> Update(T entity)
        {
            var entityType = _cafeManagerContext.Model.FindEntityType(typeof(T));
            var primaryKey = entityType?.FindPrimaryKey();
            var keyProperty = primaryKey?.Properties[0];
            var keyValue = keyProperty?.PropertyInfo?.GetValue(entity);

            var existingEntity = await _cafeManagerContext.Set<T>().FindAsync(keyValue);

            if (existingEntity != null)
            {
                _cafeManagerContext.Entry(existingEntity).CurrentValues.SetValues(entity);
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

        public async Task<IEnumerable<T>> AddArange(IEnumerable<T> entities)
        {
            if (entities == null || !entities.Any())
                return [];

            await _cafeManagerContext.AddRangeAsync(entities);
            return entities;
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await _cafeManagerContext.Set<T>().ToListAsync();
        }

        public async Task<T?> GetById(int id)
        {
            return await _cafeManagerContext.Set<T>().FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetAllExistedAsync()
        {
            var entities = await _cafeManagerContext.Set<T>().ToListAsync();
            var filteredEntities = entities
                .Where(entity =>
                {
                    var isDeletedProperty = typeof(T).GetProperty("Isdeleted");

                    if (isDeletedProperty != null && isDeletedProperty.PropertyType == typeof(bool?))
                    {
                        var isDeletedValue = (bool?)isDeletedProperty.GetValue(entity);
                        return isDeletedValue == false;
                    }

                    return false;
                });

            return filteredEntities;
        }

        public async Task<IEnumerable<T>> GetAllDeletedAsync()
        {
            var entities = await _cafeManagerContext.Set<T>().ToListAsync();
            var filteredEntities = entities
                .Where(entity =>
                {
                    var isDeletedProperty = typeof(T).GetProperty("Isdeleted");

                    if (isDeletedProperty != null && isDeletedProperty.PropertyType == typeof(bool?))
                    {
                        var isDeletedValue = (bool?)isDeletedProperty.GetValue(entity);
                        return isDeletedValue != false;
                    }

                    return false;
                });

            return filteredEntities;
        }

        public async Task<(IEnumerable<T> Items, int TotalCount)> GetByPageAsync(int pageIndex, int pageSize, Expression<Func<T, bool>>? filter = null)
        {
            IQueryable<T> query = _cafeManagerContext.Set<T>();

            if (filter != null)
            {
                query = query.Where(filter);
            };

            int totalCount = await query.CountAsync();
            List<T> items = await query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();

            return (items, totalCount);
        }
    }
}
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

        public virtual async Task<T> Create(T entity, CancellationToken token = default)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                EntityEntry<T> entityEntry = await _cafeManagerContext.Set<T>().AddAsync(entity, token);
                return entityEntry.Entity;
            }
            catch (OperationCanceledException)
            {
                throw new OperationCanceledException();
            }
        }

        public virtual async Task<T?> Update(T entity, CancellationToken token = default)
        {
            try
            {
                var entityType = _cafeManagerContext.Model.FindEntityType(typeof(T));
                var primaryKey = entityType?.FindPrimaryKey();
                var keyProperty = primaryKey?.Properties[0];
                var keyValue = keyProperty?.PropertyInfo?.GetValue(entity);
                token.ThrowIfCancellationRequested();
                var existingEntity = await _cafeManagerContext.Set<T>().FindAsync(keyValue, token);

                if (existingEntity != null)
                {
                    _cafeManagerContext.Entry(existingEntity).CurrentValues.SetValues(entity);
                }
                return entity;
            }
            catch (OperationCanceledException)
            {
                throw new OperationCanceledException();
            }
        }

        public virtual async Task<bool> Delete(int id, CancellationToken token = default)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                var entity = await _cafeManagerContext.Set<T>().FindAsync(id, token);
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
            catch (OperationCanceledException)
            {
                throw new OperationCanceledException();
            }
        }

        public async Task<IEnumerable<T>> AddArange(IEnumerable<T> entities, CancellationToken token = default)
        {
            try
            {
                if (entities == null || !entities.Any())
                    return [];
                token.ThrowIfCancellationRequested();
                await _cafeManagerContext.AddRangeAsync(entities, token);
                return entities;
            }
            catch (OperationCanceledException)
            {
                throw new OperationCanceledException();
            }
        }

        public async Task<IEnumerable<T>> GetAll(CancellationToken token = default)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                return await _cafeManagerContext.Set<T>().ToListAsync(token);
            }
            catch (OperationCanceledException)
            {
                throw new OperationCanceledException();
            }
        }

        public async Task<T?> GetById(int id, CancellationToken token = default)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                return await _cafeManagerContext.Set<T>().FindAsync(id, token);
            }
            catch (OperationCanceledException)
            {
                throw new OperationCanceledException();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<T>> GetAllExistedAsync(CancellationToken token = default)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                var entities = await _cafeManagerContext.Set<T>().ToListAsync(token);
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
            catch (OperationCanceledException)
            {
                throw new OperationCanceledException();
            }
        }

        public async Task<IEnumerable<T>> GetAllDeletedAsync(CancellationToken token = default)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                var entities = await _cafeManagerContext.Set<T>().ToListAsync(token);
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
            catch (OperationCanceledException)
            {
                throw new OperationCanceledException();
            }
        }

        public async Task<(IEnumerable<T> Items, int TotalCount)> GetByPageAsync(int pageIndex, int pageSize, Expression<Func<T, bool>>? filter = null, CancellationToken token = default)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                IQueryable<T> query = _cafeManagerContext.Set<T>();

                if (filter != null)
                {
                    query = query.Where(filter);
                };

                int totalCount = await query.CountAsync(token);
                List<T> items = await query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync(token);

                return (items, totalCount);
            }
            catch (OperationCanceledException)
            {
                throw new OperationCanceledException();
            }
            catch (Exception ex)
            {
                return (Enumerable.Empty<T>(), 0);
            }
        }
    }
}
using Alpha.Framework.MediatR.Data.Repositories;
using Alpha.Framework.MediatR.EventSourcing.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Alpha.Data
{
    public class AlphaRepository<TEntity> : IRepository<TEntity> where TEntity : Entity<TEntity>
    {
        protected AlphaDataContext _dataContext;

        public AlphaRepository(AlphaDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async virtual Task AddAsync(TEntity obj)
        {
            await _dataContext.Set<TEntity>().AddAsync(obj);
        }

        public async virtual Task<TEntity> GetById(TypedIdValueBase id)
        {
            return await _dataContext.Set<TEntity>().FindAsync(id);
        }

        public async virtual Task AddRangeAsync(List<TEntity> objs)
        {
            await _dataContext.Set<TEntity>().AddRangeAsync(objs);
        }

        public async Task UpdateAsync(TEntity existingObj, TEntity obj)
        {
            _dataContext.Entry(existingObj).CurrentValues.SetValues(obj);
            await _dataContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(TEntity entity, List<string> updatedProperties)
        {
            EntityEntry dbEntityEntry = _dataContext.Entry(entity);

            foreach (var property in dbEntityEntry.OriginalValues.Properties)
            {
                if (updatedProperties.Contains(property.Name))
                {
                    dbEntityEntry.Property(property.Name).IsModified = true;
                }
                else
                {
                    dbEntityEntry.Property(property.Name).IsModified = false;
                }
            }

            await _dataContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(TEntity obj)
        {
            _dataContext.Entry(obj).State = EntityState.Modified;
            await _dataContext.SaveChangesAsync();
        }

        public async Task UpdateRangeAsync(List<TEntity> objs)
        {
            foreach (var obj in objs)
            {
                _dataContext.Entry(obj).State = EntityState.Modified;
            }

            await _dataContext.SaveChangesAsync();
        }

        public async Task RemoveAsync(TEntity obj)
        {
            try
            {
                _dataContext.Set<TEntity>().Remove(obj);
                await _dataContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {

            }
        }

        public async Task<bool> ExistsByWhereCondition(
          Expression<Func<TEntity, bool>> predicate
        )
        {
            var count = await _dataContext.Set<TEntity>().LongCountAsync(predicate);
            return count > 0;
        }

        public void Dispose()
        {
            _dataContext.Dispose();
        }
    }
}

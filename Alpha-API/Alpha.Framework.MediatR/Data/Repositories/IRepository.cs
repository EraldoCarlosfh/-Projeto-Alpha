using Alpha.Framework.MediatR.EventSourcing.Entity;
using System.Linq.Expressions;

namespace Alpha.Framework.MediatR.Data.Repositories
{
    public interface IRepository<TEntity> : IDisposable
         where TEntity : Entity<TEntity>
    {
        Task<TEntity> GetById(TypedIdValueBase id);
        Task AddRangeAsync(List<TEntity> objs);
        Task AddAsync(TEntity obj);
        Task UpdateAsync(TEntity obj);
        Task UpdateRangeAsync(List<TEntity> objs);
        Task UpdateAsync(TEntity existingObj, TEntity obj);
        Task UpdateAsync(TEntity entity, List<string> updatedProperties);
        Task RemoveAsync(TEntity obj);
        Task<bool> ExistsByWhereCondition(Expression<Func<TEntity, bool>> predicate);
    }
}

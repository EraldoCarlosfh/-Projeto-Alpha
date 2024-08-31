using Alpha.Framework.MediatR.EventSourcing.Entity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Alpha.Framework.MediatR.Data.Queries
{
    public interface IQuery<TEntity> : IDisposable where TEntity : Entity<TEntity>
    {
        Task<TEntity> GetById(EntityId id);
        Task<List<TEntity>> GetAll();
        Task<List<TEntity>> GetActives();
    }
}

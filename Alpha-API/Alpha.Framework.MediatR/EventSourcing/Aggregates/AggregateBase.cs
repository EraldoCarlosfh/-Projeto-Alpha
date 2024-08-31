using Alpha.Framework.MediatR.EventSourcing.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alpha.Framework.MediatR.EventSourcing.Aggregates
{
    public abstract class AggregateBase<TEntity> where TEntity : Entity<TEntity>
    {
        public AggregateBase()
        {
        }
    }
}

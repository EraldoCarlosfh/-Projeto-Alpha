using Microsoft.EntityFrameworkCore;

namespace Alpha.Framework.MediatR.EventSourcing.Domains
{
    public interface IDomainEventsDispatcher
    {
        Task DispatchEventsAsync(DbContext dbContext);
    }
}

using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Alpha.Framework.MediatR.EventSourcing.Domains
{
    public interface IDomainEventsDispatcher
    {
        Task DispatchEventsAsync(DbContext dbContext);
    }
}

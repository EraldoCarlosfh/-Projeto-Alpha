using Alpha.Framework.MediatR.EventSourcing.Entity;
using MediatR;
using System.Data.Entity;

namespace Alpha.Framework.MediatR.EventSourcing.Domains
{
    public class DomainEventsDispatcher : IDomainEventsDispatcher
    {
        private readonly IMediator _mediator;

        public DomainEventsDispatcher(
            IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task DispatchEventsAsync(DbContext dbContext)
        {
            var domainEntities = dbContext.ChangeTracker.Entries<IEntity>()
                                                        .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any())
                                                        .ToList();

            if (domainEntities != null && domainEntities.Any())
            {
                var domainEvents = domainEntities.SelectMany(x => x.Entity.DomainEvents)
                                                 .ToList();

                domainEntities.ForEach(entity => entity.Entity.ClearDomainEvents());

                foreach (var domainEvent in domainEvents)
                {
                    await _mediator.Publish(domainEvent);
                }
            }
        }
    }
}

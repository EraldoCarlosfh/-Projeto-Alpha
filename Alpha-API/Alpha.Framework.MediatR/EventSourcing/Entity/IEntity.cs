using MediatR;
using System.Collections.Generic;

namespace Alpha.Framework.MediatR.EventSourcing.Entity
{
    public interface IEntity
    {
        IReadOnlyCollection<INotification> DomainEvents { get; }
        void AddDomainEvent(INotification domainEvent);
        void ClearDomainEvents();
    }
}

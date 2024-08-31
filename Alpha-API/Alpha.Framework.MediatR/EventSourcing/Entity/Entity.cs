using Alpha.Framework.MediatR.Notifications;
using Alpha.Framework.MediatR.Resources.Extensions;
using MediatR;
using System;
using System.Collections.Generic;

namespace Alpha.Framework.MediatR.EventSourcing.Entity
{
    public abstract class Entity<T> : OctaNotifiable, IEntity where T : Entity<T>
    {
        public Entity()
        {
            Id = new EntityId(Guid.NewGuid().ToString());
            CreatedAt = DateTime.UtcNow.ToLocalTimeZone();
        }

        public EntityId Id { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        private List<INotification> _notifications;

        public static T New()
        {
            var newEntity = Activator.CreateInstance(typeof(T)) as T;
            var guidString = Guid.NewGuid().ToString();
            newEntity.Id = new EntityId(guidString);
            return newEntity;
        }

        public static T NewWith(EntityId id)
        {
            var newEntity = Activator.CreateInstance(typeof(T)) as T;
            newEntity.Id = id;
            return newEntity;
        }

        /// <summary>
        /// Domain events occurred.
        /// </summary>
        public IReadOnlyCollection<INotification> DomainEvents => _notifications?.AsReadOnly();

        public void AddDomainEvent(INotification domainEvent)
        {
            _notifications ??= new List<INotification>();
            _notifications.Add(domainEvent);
        }
        /// <summary>
        /// Cvehicle domain events.
        /// </summary>
        public void ClearDomainEvents()
        {
            _notifications?.Clear();
        }
    }
}

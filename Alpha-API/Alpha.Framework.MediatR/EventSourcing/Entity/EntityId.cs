using System;

namespace Alpha.Framework.MediatR.EventSourcing.Entity
{
    public class EntityId : TypedIdValueBase
    {
        public EntityId(string value) : base(value)
        {
        }

        public object ToEntityId()
        {
            throw new NotImplementedException();
        }
    }
}

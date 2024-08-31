using Alpha.Framework.MediatR.Notifications;
using MediatR;

namespace Alpha.Framework.MediatR.EventSourcing.Events
{
    public abstract class Event : OctaNotifiable, INotification
    {
        public abstract bool IsValid();
    }
}

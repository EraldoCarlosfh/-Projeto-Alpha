using Microsoft.EntityFrameworkCore;

namespace Alpha.Framework.MediatR.Notifications
{
    [Keyless]
    public sealed class Notification
    {
        public Notification(string property, string message)
        {
            Property = property;
            Message = message;
        }

        public string Property { get; private set; }
        public string Message { get; private set; }
    }
}

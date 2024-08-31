using Alpha.Framework.MediatR.Notifications;

namespace Alpha.Framework.MediatR.Validations
{
    public partial class ValidationContract : OctaNotifiable
    {
        public ValidationContract Requires()
        {
            return this;
        }

        public ValidationContract Concat(OctaNotifiable notifiable)
        {
            if (notifiable.Invalid)
                AddNotifications(notifiable.Notifications);

            return this;
        }
    }
}

using System.ComponentModel.DataAnnotations.Schema;
using FluentValidation.Results;
using System.Text.Json.Serialization;

namespace Alpha.Framework.MediatR.Notifications
{
    public abstract class OctaNotifiable
    {
        public readonly List<Notification> _notifications;

        [NotMapped]
        [JsonIgnore]
        public ValidationResult ValidationResult { get; protected set; }

        [NotMapped]
        [JsonIgnore]
        public IReadOnlyCollection<Notification> Notifications =>
            new List<Notification>(_notifications).Concat(GetNotificationsFromValidations()).ToList();

        protected OctaNotifiable() { _notifications = new List<Notification>(); }

        public List<Notification> AddNotification(string property, string message)
        {
            _notifications.Add(new Notification(property, message));
            return _notifications;
        }

        public List<Notification> AddNotification(Notification notification)
        {
            _notifications.Add(notification);
            return _notifications;
        }

        public List<Notification> AddNotifications(IReadOnlyCollection<Notification> notifications)
        {
            _notifications.AddRange(notifications);
            return _notifications;
        }

        public List<Notification> AddNotifications(IList<Notification> notifications)
        {
            _notifications.AddRange(notifications);
            return _notifications;
        }

        public List<Notification> AddNotifications(ICollection<Notification> notifications)
        {
            _notifications.AddRange(notifications);
            return _notifications;
        }

        public void AddNotifications(OctaNotifiable item)
        {
            AddNotifications(item.Notifications);
        }

        public void AddNotifications(params OctaNotifiable[] items)
        {
            foreach (var item in items)
            {
                AddNotifications(item);
            }
        }

        public void ClearNotifications()
        {
            _notifications.Clear();
        }

        protected virtual IEnumerable<Notification> Validations() => null;

        private IEnumerable<Notification> GetNotificationsFromValidations()
        {
            return Validations() ?? new List<Notification>();
        }

        protected void NotifyValidationErros()
        {
            foreach (var error in ValidationResult?.Errors)
            {
                AddNotification(error.PropertyName, error.ErrorMessage);
            }
        }

        [JsonIgnore]
        public bool Invalid => _notifications.Any() || GetNotificationsFromValidations().Any();

        [JsonIgnore]
        public bool Valid => !Invalid;
    }
}

using Alpha.Framework.MediatR.Notifications;

namespace Alpha.Domain.Commands
{
    public class CommandResult<T> : ICommandResult<T>
    {
        public bool IsSuccess { get; private set; }
        public string Message { get; private set; }
        public T Data { get; private set; }
        public List<Notification> Notifications { get; private set; }

        public CommandResult(bool isSuccess, string message, IReadOnlyCollection<Notification> notifications)
        {
            if (Notifications == null)
                Notifications = new List<Notification>();

            IsSuccess = isSuccess;
            Message = message;
            Notifications.AddRange(notifications);
        }


        public CommandResult(bool isSuccess, string message, IReadOnlyCollection<Notification> notifications, T data)
        {
            if (Notifications == null)
                Notifications = new List<Notification>();

            IsSuccess = isSuccess;
            Message = message;
            Notifications.AddRange(notifications);
            Data = data;
        }

        public CommandResult(bool isSuccess, string message, T data)
        {
            IsSuccess = isSuccess;
            Message = message;
            Data = data;
        }
    }
}


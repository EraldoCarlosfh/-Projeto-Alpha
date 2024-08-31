using Alpha.Framework.MediatR.Notifications;

namespace Alpha.Framework.MediatR.EventSourcing.Commands
{
    public interface ICommandResult<T>
    {
        bool IsSuccess { get; }
        string Message { get; }
        List<Notification> Notifications { get; }
        T Data { get; }
    }
}

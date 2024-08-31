using Alpha.Framework.MediatR.Notifications;
using System.Collections.Generic;

namespace Alpha.Domain.Commands
{
    public interface ICommandResult<T>
    {
        bool IsSuccess { get; }
        string Message { get; }
        List<Notification> Notifications { get; }
        T Data { get; }
    }
}

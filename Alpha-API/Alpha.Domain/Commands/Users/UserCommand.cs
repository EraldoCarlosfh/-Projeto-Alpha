using Alpha.Domain.Entities;
using Alpha.Framework.MediatR.Notifications;
using MediatR;

namespace Alpha.Domain.Commands.Users
{
    public class UserCommand : OctaNotifiable, IRequest<ICommandResult<User>>
    {
        public string DocumentNumber { get; set; }

        public string Email { get; set; }

        public DateTime? Birthdate { get; set; }

        public List<string> PersonIds { get; set; }
    }
}

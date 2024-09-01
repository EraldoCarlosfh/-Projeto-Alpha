using Alpha.Domain.Commands.Users;
using Alpha.Framework.MediatR.EventSourcing.Commands;
using FluentValidation;

namespace Alpha.Domain.Validators.Commands.Users
{
    public class LoginUserCommandValidator : CommandValidator<LoginUserCommand>
    {
        public LoginUserCommandValidator()
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage("E-mail não informado.");

            RuleFor(x => x.Password).NotEmpty().WithMessage("Senha não informada.");
        }
    }
}

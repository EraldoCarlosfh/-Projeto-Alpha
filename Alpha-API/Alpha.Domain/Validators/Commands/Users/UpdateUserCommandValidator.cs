using Alpha.Domain.Commands.Users;
using Alpha.Domain.Extensions.Commons;
using Alpha.Framework.MediatR.EventSourcing.Commands;
using FluentValidation;
using Alpha.Domain.Validators;

namespace Alpha.Domain.Validators.Commands.Users
{
    public class UpdateUserCommandValidator : CommandValidator<UpdateUserCommand>
    {
        public UpdateUserCommandValidator()
        {
            RuleFor(x => x.FullName).NotEmpty().WithMessage("Nome é obrigatório.");
            RuleFor(x => x.Email).NotEmpty().WithMessage("E-mail é obrigatório.");
        }
    }
}

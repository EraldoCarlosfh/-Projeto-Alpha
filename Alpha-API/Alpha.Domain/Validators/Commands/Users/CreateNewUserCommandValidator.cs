using Alpha.Domain.Commands.Users;
using Alpha.Domain.Entities;
using Alpha.Domain.Extensions.Commons;
using Alpha.Framework.MediatR.Data.Repositories;
using Alpha.Framework.MediatR.EventSourcing.Commands;
using FluentValidation;
using Alpha.Domain.Validators;
using System.Threading;
using System.Threading.Tasks;

namespace Alpha.Domain.Validators.Commands.Users
{
    public class CreateNewUserCommandValidator : CommandValidator<CreateNewUserCommand>
    {
        public CreateNewUserCommandValidator() : base()
        {
            RuleFor(x => x.FullName).NotEmpty().WithMessage("Informe o Nome Completo.");

            RuleFor(x => x.Password).NotEmpty().WithMessage("Senha é obrigatória.");
   
            RuleFor(x => x.Email).NotEmpty().WithMessage("E-mail é obrigatório.")
                .EmailAddress().WithMessage("Informe um e-mail válido.");
        }
    }
}

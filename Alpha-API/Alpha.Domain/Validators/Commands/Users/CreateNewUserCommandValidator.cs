using Alpha.Domain.Commands.Users;
using Alpha.Domain.Entities;
using Alpha.Domain.Extensions.Commons;
using Alpha.Framework.MediatR.Data.Repositories;
using Alpha.Framework.MediatR.EventSourcing.Commands;
using FluentValidation;
using OctaTech.Domain.Validators;

namespace Alpha.Domain.Validators.Commands.Users
{
    public class CreateNewUserCommandValidator : CommandValidator<CreateNewUserCommand>
    {
        public CreateNewUserCommandValidator() : base()
        {
            RuleFor(x => x.FullName).NotEmpty().WithMessage(ValidationMessages.RequiredField.Formater("Nome Completo"));

            RuleFor(x => x.Password).NotEmpty().WithMessage(ValidationMessages.RequiredField.Formater("Senha"));

            RuleFor(x => x.IsAcceptUseTerms).Must(x => x == true).WithMessage("Usuário não aceitou os termos de uso");
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage(ValidationMessages.RequiredField.Formater("E-mail"))
                .EmailAddress().WithMessage(ValidationMessages.InvalidField.Formater("E-mail"))
                .MustAsync(EmailUserMustBeUnique).WithMessage(ValidationMessages.UniqueField.Formater("E-mail"));
        }

        public async Task<bool> EmailUserMustBeUnique(string email, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await ServiceWrapper<IRepository<User>, bool>(
                async repository =>
                {
                    var resultado = await repository.ExistsByWhereCondition(
                        entidade => entidade.Email == email
                    );

                    return !resultado;
                }
            );
        }
    }
}

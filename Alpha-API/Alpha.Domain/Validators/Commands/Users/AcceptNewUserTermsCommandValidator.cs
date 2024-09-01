using Alpha.Domain.Commands.Users;
using Alpha.Domain.Entities;
using Alpha.Domain.Extensions.Commons;
using Alpha.Framework.MediatR.Data.Repositories;
using Alpha.Framework.MediatR.EventSourcing.Commands;
using Alpha.Framework.MediatR.Resources.Extensions;
using FluentValidation;
using Alpha.Domain.Validators;
using System.Threading;
using System.Threading.Tasks;

namespace Alpha.Domain.Validators.Commands.Users
{
    public class AcceptNewUserTermsCommandValidator : CommandValidator<AcceptNewUserTermsCommand>
    {
        public AcceptNewUserTermsCommandValidator()
        {
            RuleFor(x => x.UserId).NotEmpty().WithMessage(ValidationMessages.RequiredField.Formater("Id do Usuário"))
               .MustAsync(UserMustExist).WithMessage(ValidationMessages.NotFound.Formater("Usuário"));

            RuleFor(x => x.ActivationOperationalSystem).NotEmpty().WithMessage(ValidationMessages.RequiredField.Formater("Ativação do Sistema Operacional"));
            RuleFor(x => x.ActivationIp).NotEmpty().WithMessage(ValidationMessages.RequiredField.Formater("IP de ativação"));
            RuleFor(x => x.ActivationBrowser).NotEmpty().WithMessage(ValidationMessages.RequiredField.Formater("Browser de ativação"));
        }

        public async Task<bool> UserMustExist(string userId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await ServiceWrapper<IRepository<User>, bool>(
                async repository =>
                {
                    var resultado = await repository.ExistsByWhereCondition(
                        entidade => entidade.Id == userId.ToEntityId()
                    );

                    return resultado;
                }
            );
        }
    }
}

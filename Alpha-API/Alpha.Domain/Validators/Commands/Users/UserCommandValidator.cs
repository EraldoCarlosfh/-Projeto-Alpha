using Alpha.Domain.Commands.Users;
using Alpha.Domain.Entities;
using Alpha.Domain.Extensions.Commons;
using Alpha.Framework.MediatR.Data.Repositories;
using Alpha.Framework.MediatR.EventSourcing.Commands;
using Alpha.Framework.MediatR.Resources.Extensions;
using Alpha.Integrations.Encryption;
using FluentValidation;
using Alpha.Domain.Validators;
using System.Threading;
using System.Threading.Tasks;

namespace Alpha.Domain.Validators.Commands.Users
{
    public class UserCommandValidator<T> : CommandValidator<T> where T : UserCommand
    {
        public UserCommandValidator()
        {
            RuleFor(x => x.DocumentNumber).NotEmpty().WithMessage(ValidationMessages.RequiredField.Formater("Número de Documento"))
                .Must(x => x.IsValidCPF()).WithMessage(ValidationMessages.InvalidField.Formater("Número de Documento"))
                .MustAsync(DocumentUserMustBeUnique).WithMessage(ValidationMessages.UniqueField.Formater("Número de Documento"));

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage(ValidationMessages.RequiredField.Formater("E-mail"))
                .EmailAddress().WithMessage(ValidationMessages.InvalidField.Formater("E-mail"))
                .MustAsync(EmailUserMustBeUnique).WithMessage(ValidationMessages.UniqueField.Formater("E-mail"));

        }

        public async Task<bool> DocumentUserMustBeUnique(string document, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await ServiceWrapper<IRepository<User>, IEncryptionService, bool>(
                async (repository, _encryptionService) =>
                {
                    var resultado = await repository.ExistsByWhereCondition(
                        entidade => entidade.EncryptedDocumentNumber == _encryptionService.Encrypt(document.ClearNumberMask())
                    );

                    return !resultado;
                }
            );
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

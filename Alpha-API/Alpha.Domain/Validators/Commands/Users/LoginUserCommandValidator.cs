using Alpha.Domain.Commands.Users;
using Alpha.Domain.Entities;
using Alpha.Domain.Extensions.Commons;
using Alpha.Domain.Queries.Users;
using Alpha.Framework.MediatR.EventSourcing.Commands;
using Alpha.Framework.MediatR.Resources.Extensions;
using FluentValidation;
using OctaTech.Domain.Validators;

namespace Alpha.Domain.Validators.Commands.Users
{
    public class LoginUserCommandValidator : CommandValidator<LoginUserCommand>
    {
        public LoginUserCommandValidator()
        {
            RuleFor(x => x.UserIdentification).NotEmpty().WithMessage(ValidationMessages.RequiredField.Formater("Identificação do usuário"));

            RuleFor(x => x).MustAsync(UserMustExist).WithMessage(ValidationMessages.NotFound.Formater("Usuário"));

            RuleFor(x => x.IdentificationType).NotEmpty().WithMessage(ValidationMessages.RequiredField.Formater("Tipo da identificação"))
               .Must(StringValueOptions).WithMessage("O tipo de operação não é válida. Opções válidas: \"Email\" e \"Cpf\" e \"Celular\" e \"CODIGO_CELULAR\"");

            RuleFor(x => x).Must(ValidatePassword).WithMessage(ValidationMessages.RequiredField.Formater("Senha"));
        }

        public bool StringValueOptions(string identificationType)
        {
            var identification = identificationType.ToUpper();
            return identification == LoginUserCommand.IDENTIFICATION_TYPE_EMAIL ||
                   identification == LoginUserCommand.IDENTIFICATION_TYPE_CPF ||
                   identification == LoginUserCommand.IDENTIFICATION_TYPE_CELULAR ||
                   identification == LoginUserCommand.IDENTIFICATION_TYPE_CODIGO_CELULAR;
        }

        public async Task<bool> UserMustExist(LoginUserCommand loginUser, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await ServiceWrapper<IUserQuery, bool>(
               async _userQuery =>
               {
                   User user = null;
                   switch (loginUser.IdentificationType.ToUpper())
                   {
                       case LoginUserCommand.IDENTIFICATION_TYPE_EMAIL:
                           user = await _userQuery.GetUserByEmail(loginUser.UserIdentification.ToLower());
                           break;
                       case LoginUserCommand.IDENTIFICATION_TYPE_CPF:
                           user = await _userQuery.GetUserByDocumentNumber(loginUser.UserIdentification.ClearNumberMask());
                           break;
                       case LoginUserCommand.IDENTIFICATION_TYPE_CELULAR:
                           user = await _userQuery.GetUserByCellPhone(loginUser.UserIdentification.ClearNumberMask());
                           break;
                       case LoginUserCommand.IDENTIFICATION_TYPE_CODIGO_CELULAR:
                           user = await _userQuery.GetUserByLoginPhoneCode(loginUser.UserIdentification.ClearNumberMask());
                           break;
                   }

                   return user != null;
               }
            );
        }

        public async Task<bool> UserMustBeApproved(LoginUserCommand loginUser, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await ServiceWrapper<IUserQuery, bool>(
                async _userQuery =>
                {
                    User user = null;
                    switch (loginUser.IdentificationType.ToUpper())
                    {
                        case LoginUserCommand.IDENTIFICATION_TYPE_EMAIL:
                            user = await _userQuery.GetUserByEmail(loginUser.UserIdentification.ToLower());
                            break;
                        case LoginUserCommand.IDENTIFICATION_TYPE_CPF:
                            user = await _userQuery.GetUserByDocumentNumber(loginUser.UserIdentification.ClearNumberMask());
                            break;
                        case LoginUserCommand.IDENTIFICATION_TYPE_CELULAR:
                            user = await _userQuery.GetUserByCellPhone(loginUser.UserIdentification.ClearNumberMask());
                            break;
                        case LoginUserCommand.IDENTIFICATION_TYPE_CODIGO_CELULAR:
                            user = await _userQuery.GetUserByLoginPhoneCode(loginUser.UserIdentification.ClearNumberMask());
                            break;
                    }

                    return user != null && user.IsAccessActivated.HasValue && user.IsAccessActivated.Value && (user.EmailActivationCode != null || user.CellPhoneActivationCode != null);
                }
            );
        }

        public bool ValidatePassword(LoginUserCommand loginUser)
        {
            if (loginUser.IdentificationType.ToUpper() == LoginUserCommand.IDENTIFICATION_TYPE_CODIGO_CELULAR)
                return true;

            return !string.IsNullOrEmpty(loginUser.SaltPassword);
        }
    }
}

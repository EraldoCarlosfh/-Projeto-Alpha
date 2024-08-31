using Alpha.Domain.Entities;
using Alpha.Domain.Queries.Users;
using Alpha.Domain.Validators.Commands.Users;
using Alpha.Framework.MediatR.Data.Repositories;
using Alpha.Framework.MediatR.Data.UnitOfWork;
using Alpha.Framework.MediatR.Notifications;
using Alpha.Framework.MediatR.Resources.Extensions;
using Alpha.Framework.MediatR.SecurityService.DataTransferObjects;
using Alpha.Framework.MediatR.SecurityService.Models;
using Alpha.Framework.MediatR.SecurityService;
using Alpha.Integrations.Encryption;
using MediatR;
using Alpha.Domain.Responses.Users;

namespace Alpha.Domain.Commands.Users
{
    public class LoginUserCommand : OctaNotifiable, IRequest<ICommandResult<LoginUserResponse>>
    {
        public const string IDENTIFICATION_TYPE_EMAIL = "EMAIL";
        public const string IDENTIFICATION_TYPE_CPF = "CPF";
        public const string IDENTIFICATION_TYPE_CELULAR = "CELULAR";
        public const string IDENTIFICATION_TYPE_CODIGO_CELULAR = "CODIGO_CELULAR";

        public string IdentificationType { get; set; }
        public string UserIdentification { get; set; }
        public string SaltPassword { get; set; }

        public bool IsValid()
        {
            ValidationResult = new LoginUserCommandValidator().Validate(this);
            NotifyValidationErros();
            return ValidationResult.IsValid;
        }
    }

    public class LoginUserRequestHandler : CommandHandlerBase<User, LoginUserCommand, ICommandResult<LoginUserResponse>>
    {
        private readonly IAuthService _tokenService;
        private readonly IRepository<User> _userRepository;
        private readonly IEncryptionService _encryptionService;

        public LoginUserRequestHandler(
            IUnitOfWork unitOfWork,
            AuthenticatedUserModel authenticatedUser,
            IUserQuery userQuery,        
            IAuthService tokenService,
            IRepository<User> userRepository,
            IEncryptionService encryptionService) : base(unitOfWork, userQuery, authenticatedUser)
        {
            _tokenService = tokenService;
            _userRepository = userRepository;
            _encryptionService = encryptionService;
        }

        protected override async Task<ICommandResult<LoginUserResponse>> HandleRequest(LoginUserCommand command, CancellationToken cancellationToken)
        {
            if (!command.IsValid())
                return new CommandResult<LoginUserResponse>(false, "Errors", command.Notifications);

            User user = null;
            switch (command.IdentificationType.ToUpper())
            {
                case LoginUserCommand.IDENTIFICATION_TYPE_EMAIL:
                    user = await _userQuery.GetUserByEmail(command.UserIdentification.ToLower());
                    break;
                case LoginUserCommand.IDENTIFICATION_TYPE_CPF:
                    user = await _userQuery.GetUserByDocumentNumber(command.UserIdentification.ClearNumberMask());
                    break;
                case LoginUserCommand.IDENTIFICATION_TYPE_CELULAR:
                    user = await _userQuery.GetUserByCellPhone(command.UserIdentification.ClearNumberMask());
                    break;
                case LoginUserCommand.IDENTIFICATION_TYPE_CODIGO_CELULAR:
                    user = await _userQuery.GetUserByLoginPhoneCode(command.UserIdentification.ClearNumberMask());
                    break;
            }

            if (user == null)
            {
                return await HandleUserNotExistError(command);
            }

            if (user.IsAccessActivated != true)
            {
                return await HandleActivateError(command, user);
            }
           
            var userEncryptedPasswordResponse = _encryptionService.GenerateEncryptedPassword(command.SaltPassword);
            if (!userEncryptedPasswordResponse.IsSuccess)
            {
                return await HandlePasswordError(command, user);
            }

            var userEncryptedPassword = _encryptionService.Encrypt(userEncryptedPasswordResponse.Password);
            if (command.IdentificationType.ToUpper() != LoginUserCommand.IDENTIFICATION_TYPE_CODIGO_CELULAR && !userEncryptedPassword.Equals(user.Password))
            {
                return await HandlePasswordError(command, user);
            }
            else
            {
                user.Login();

                if (command.IdentificationType.ToUpper() == LoginUserCommand.IDENTIFICATION_TYPE_CODIGO_CELULAR)
                    user.RemoveLoginPhoneCode();

                await _userRepository.UpdateAsync(user);

                bool? isRegisteredDetran, enabledRE, isExpirationDate;
                bool isApprovedNotificationViewed;
              

                var tokenRequest = new CreateTokenRequest()
                {                  
                    Email = user.Email ?? user.CellPhone,
                    Id = user.Id.ToString(),
                    Name = $"{user.FirstName} {user.LastName}"
                };

                var tokenResponse = _tokenService.CreateToken(tokenRequest);

                var response = new LoginUserResponse()
                {
                    ExpirationDate = tokenResponse.ExpirationDate,
                    Token = tokenResponse.Token,               
                    User = user,
                    UserId = user.Id.ToString(),
                    PasswordErrorsAllowed = user.PasswordErrorsAllowed,
                    IsAccessActivated = user.IsAccessActivated,
                    IsNewNotification = user.IsNewNotification
                };

                return new CommandResult<LoginUserResponse>(true, "Success", response);
            }
        }       

        private async Task<ICommandResult<LoginUserResponse>> HandleUserNotExistError(LoginUserCommand command)
        {

            command.AddNotification("Erro", "Usuário não encontrado");
            var retorno = new CommandResult<LoginUserResponse>(false, "Errors", command.Notifications,
                new LoginUserResponse
                {
                    UserExists = false,
                    IsAccessActivated = true
                });
            return retorno;
        }

        private async Task<ICommandResult<LoginUserResponse>> HandlePasswordError(LoginUserCommand command, User user)
        {
            user.HandlePasswordError();

            await _userRepository.UpdateAsync(user);
            await CommitPendingChanges();

            command.AddNotification("Erro", "Senha inválida");
            var retorno = new CommandResult<LoginUserResponse>(false, "Errors", command.Notifications,
                new LoginUserResponse
                {
                    PasswordErrorsAllowed = user.PasswordErrorsAllowed,
                    IsAccessActivated = user.IsAccessActivated,
                    IsApproved = true
                });
            return retorno;
        }

        private async Task<ICommandResult<LoginUserResponse>> HandleActivateError(LoginUserCommand command, User user)
        {
            var codeResponse = _tokenService.GenerateVerificationCode(user.Email ?? user.CellPhone);

            user.ResendActivationCodeByEmail(codeResponse.Code, codeResponse.ExpirationDate, false);
            command.AddNotification("Erro", "O Usuário precisa ser ativado.");
            return new CommandResult<LoginUserResponse>(false, "Errors", command.Notifications,
                new LoginUserResponse
                {
                    UserId = user.Id.ToString(),
                    PasswordErrorsAllowed = user.PasswordErrorsAllowed,
                    IsAccessActivated = user.IsAccessActivated
                });
        }

        private async Task<ICommandResult<LoginUserResponse>> HandleActivatePersonError(LoginUserCommand command, User user)
        {
            command.AddNotification("Erro", "A empresa ainda não foi validada pelos gestores Octa.");
            return new CommandResult<LoginUserResponse>(false, "Errors", command.Notifications,
                new LoginUserResponse
                {
                    UserId = user.Id.ToString(),
                    PasswordErrorsAllowed = user.PasswordErrorsAllowed,
                    IsAccessActivated = user.IsAccessActivated
                });
        }

        private async Task<ICommandResult<LoginUserResponse>> HandleDeclinePersonError(LoginUserCommand command, User user)
        {
            command.AddNotification("Erro", "O cadastro da empresa não foi aceito pelos gestores Octa.");
            return new CommandResult<LoginUserResponse>(false, "Errors", command.Notifications,
                new LoginUserResponse
                {
                    UserId = user.Id.ToString(),
                    PasswordErrorsAllowed = user.PasswordErrorsAllowed,
                    IsAccessActivated = user.IsAccessActivated
                });
        }
    }
}

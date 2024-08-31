using Alpha.Domain.Entities;
using Alpha.Domain.Queries.Users;
using Alpha.Domain.Responses.Users;
using Alpha.Framework.MediatR.Data.Repositories;
using Alpha.Framework.MediatR.Data.UnitOfWork;
using Alpha.Framework.MediatR.Notifications;
using Alpha.Framework.MediatR.Resources.Extensions;
using Alpha.Framework.MediatR.SecurityService.Models;
using Alpha.Framework.MediatR.SecurityService;
using MediatR;
using Alpha.Domain.Validators.Commands.Users;

namespace Alpha.Domain.Commands.Users
{
    public class LoginPhoneGenerateCodeCommand : OctaNotifiable, IRequest<ICommandResult<LoginPhoneGenerateCodeResponse>>
    {
        public string PhoneNumber { get; set; }

        public bool IsValid()
        {
            ValidationResult = new LoginPhoneGenerateCodeCommandValidator().Validate(this);
            NotifyValidationErros();
            return ValidationResult.IsValid;
        }
    }

    public class LoginPhoneGenerateCodeCommandHandler : CommandHandlerBase<User, LoginPhoneGenerateCodeCommand, ICommandResult<LoginPhoneGenerateCodeResponse>>
    {
        private readonly IAuthService _tokenService;
        private readonly IRepository<User> _userRepository;

        public LoginPhoneGenerateCodeCommandHandler(
            IUnitOfWork unitOfWork,
            AuthenticatedUserModel authenticatedUser,
            IUserQuery userQuery,        
            IAuthService tokenService,
            IRepository<User> userRepository) : base(unitOfWork, userQuery, authenticatedUser)
        {
            _tokenService = tokenService;
            _userRepository = userRepository;
        }

        protected override async Task<ICommandResult<LoginPhoneGenerateCodeResponse>> HandleRequest(LoginPhoneGenerateCodeCommand command, CancellationToken cancellationToken)
        {
            if (!command.IsValid())
                return new CommandResult<LoginPhoneGenerateCodeResponse>(false, "Errors", command.Notifications);

            User user = await _userQuery.GetUserByCellPhone(command.PhoneNumber.ClearNumberMask());

            if (user == null)
            {
                return HandleUserNotExistError(command);
            }

            if (user.IsAccessActivated != true)
            {
                return HandleActivateError(command, user);
            }        

            user.GenerateLoginPhoneCode();
            await _userRepository.UpdateAsync(user);         

            return new CommandResult<LoginPhoneGenerateCodeResponse>(true, "Success", new LoginPhoneGenerateCodeResponse
            {
                IsAccessActivated = true,
                IsApproved = true,
                SentSmsCode = true,
                UserExists = true
            });
        }

        private ICommandResult<LoginPhoneGenerateCodeResponse> HandleUserNotExistError(LoginPhoneGenerateCodeCommand command)
        {

            command.AddNotification("Erro", "Usuário não encontrado");
            var retorno = new CommandResult<LoginPhoneGenerateCodeResponse>(false, "Errors", command.Notifications,
                new LoginPhoneGenerateCodeResponse
                {
                    UserExists = false,
                    IsAccessActivated = true
                });
            return retorno;
        }

        private ICommandResult<LoginPhoneGenerateCodeResponse> HandleActivateError(LoginPhoneGenerateCodeCommand command, User user)
        {
            var codeResponse = _tokenService.GenerateVerificationCode(user.Email ?? user.CellPhone);

            user.ResendActivationCodeByEmail(codeResponse.Code, codeResponse.ExpirationDate, false);
            command.AddNotification("Erro", "O Usuário precisa ser ativado.");
            return new CommandResult<LoginPhoneGenerateCodeResponse>(false, "Errors", command.Notifications,
                new LoginPhoneGenerateCodeResponse
                {
                    UserId = user.Id.ToString(),
                    IsAccessActivated = user.IsAccessActivated
                });
        }

        private ICommandResult<LoginPhoneGenerateCodeResponse> HandleActivatePersonError(LoginPhoneGenerateCodeCommand command, User user)
        {
            command.AddNotification("Erro", "A empresa ainda não foi validada pelos gestores Octa.");
            return new CommandResult<LoginPhoneGenerateCodeResponse>(false, "Errors", command.Notifications,
                new LoginPhoneGenerateCodeResponse
                {
                    UserId = user.Id.ToString(),
                    IsAccessActivated = user.IsAccessActivated
                });
        }
    }
}
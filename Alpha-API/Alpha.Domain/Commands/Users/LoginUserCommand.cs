using Alpha.Domain.Entities;
using Alpha.Domain.Queries.Users;
using Alpha.Domain.Validators.Commands.Users;
using Alpha.Framework.MediatR.Data.Repositories;
using Alpha.Framework.MediatR.Data.UnitOfWork;
using Alpha.Framework.MediatR.Notifications;
using Alpha.Framework.MediatR.SecurityService.DataTransferObjects;
using Alpha.Framework.MediatR.SecurityService.Models;
using Alpha.Framework.MediatR.SecurityService;
using Alpha.Integrations.Encryption;
using MediatR;
using Alpha.Domain.Responses.Users;
using System.Threading.Tasks;
using System.Threading;

namespace Alpha.Domain.Commands.Users
{
    public class LoginUserCommand : OctaNotifiable, IRequest<ICommandResult<LoginUserResponse>>
    {    
        public string Email { get; set; }
        public string Password { get; set; }

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
                   
            user = await _userQuery.GetUserByEmail(command.Email);

            if (user == null)
                return await HandleUserNotExistError(command);         
         
            if (user.Password != command.Password)
                return await HandlePasswordError(command, user);

            user.Login();

            await _userRepository.UpdateAsync(user);

            var tokenRequest = new CreateTokenRequest()
            {
                Email = user.Email,
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

        private async Task<ICommandResult<LoginUserResponse>> HandleUserNotExistError(LoginUserCommand command)
        {

            command.AddNotification("Erro", "Usuário não encontrado");
            var retorno = new CommandResult<LoginUserResponse>(false, "Errors", command.Notifications,
                new LoginUserResponse
                {
                    UserExists = false
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
                    IsAccessActivated = user.IsAccessActivated
                });
            return retorno;
        }        
    }
}

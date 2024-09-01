using Alpha.Domain.Entities;
using Alpha.Framework.MediatR.Data.Repositories;
using Alpha.Framework.MediatR.Data.UnitOfWork;
using Alpha.Framework.MediatR.Notifications;
using Alpha.Framework.MediatR.SecurityService.Models;
using Alpha.Framework.MediatR.SecurityService;
using MediatR;
using Alpha.Domain.Validators.Commands.Users;
using Alpha.Domain.Queries.Users;
using Alpha.Integrations.Encryption;
using System.Threading.Tasks;
using System.Threading;

namespace Alpha.Domain.Commands.Users
{
    public class CreateNewUserCommand : OctaNotifiable, IRequest<ICommandResult<User>>
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public bool IsValid()
        {
            ValidationResult = new CreateNewUserCommandValidator().Validate(this);
            NotifyValidationErros();
            return ValidationResult.IsValid;
        }
    }

    public class CreateNewUserRequestHandler : CommandHandlerBase<User, CreateNewUserCommand, ICommandResult<User>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IRepository<User> _userRepository;
        private readonly IAuthService _authenticationService;
        private readonly IEncryptionService _encryptionService;

        public CreateNewUserRequestHandler(
            IUnitOfWork uow,
            IUnitOfWork unitOfWork,
            AuthenticatedUserModel authenticatedUser,
            IRepository<User> userRepository,
            IUserQuery userQuery,
            IEncryptionService encryptionService,
            IAuthService authenticationService) : base(unitOfWork, userQuery, authenticatedUser)
        {
            _uow = uow;
            _userRepository = userRepository;
            _authenticationService = authenticationService;
            _encryptionService = encryptionService;
        }

        protected override async Task<ICommandResult<User>> HandleRequest(CreateNewUserCommand command, CancellationToken cancellationToken)
        {
            if (!command.IsValid())
                return ValidationErrors(command.Notifications);
            
            var codeResponseEmail = _authenticationService.GenerateVerificationCode(command.Email.ToLower());

            var user = await _userQuery.GetUserByEmail(command.Email);

            if (user != null) {
                return ValidationErrors(command.AddNotification(new Notification("Cadastro", "E-mail já cadastrado na base de dados.")));
            }

            user = User.Create(
                command,
                command.Password,
                codeResponseEmail.Code,
                codeResponseEmail.ExpirationDate);

            await _userRepository.AddAsync(user);

            return Success(user);
        }
    }
}
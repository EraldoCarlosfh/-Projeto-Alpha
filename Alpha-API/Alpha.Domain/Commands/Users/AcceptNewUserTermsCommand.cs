using Alpha.Domain.Entities;
using Alpha.Domain.Queries.Users;
using Alpha.Domain.Responses.Users;
using Alpha.Framework.MediatR.Data.Repositories;
using Alpha.Framework.MediatR.Data.UnitOfWork;
using Alpha.Framework.MediatR.Notifications;
using Alpha.Framework.MediatR.Resources.Extensions;
using Alpha.Framework.MediatR.SecurityService.DataTransferObjects;
using Alpha.Framework.MediatR.SecurityService.Models;
using Alpha.Framework.MediatR.SecurityService;
using Alpha.Integrations.Encryption;
using MediatR;
using Alpha.Domain.Validators.Commands.Users;

namespace Alpha.Domain.Commands.Users
{
    public class AcceptNewUserTermsCommand : OctaNotifiable, IRequest<ICommandResult<LoginUserResponse>>
    {
        public string UserId { get; set; }
        public string ActivationIp { get; set; }
        public string ActivationBrowser { get; set; }
        public string ActivationOperationalSystem { get; set; }

        public bool IsValid()
        {
            ValidationResult = new AcceptNewUserTermsCommandValidator().Validate(this);
            NotifyValidationErros();
            return ValidationResult.IsValid;
        }

    }

    public class AcceptNewUserTermsRequestHandler : CommandHandlerBase<User, AcceptNewUserTermsCommand, ICommandResult<LoginUserResponse>>
    {
        private readonly IAuthService _tokenService;
        private readonly IRepository<User> _userRepository;
        private readonly IEncryptionService _encryptionService;

        public AcceptNewUserTermsRequestHandler(
            IAuthService tokenService,
            IUnitOfWork unitOfWork,
            AuthenticatedUserModel authenticatedUser,
            IUserQuery userQuery,      
            IRepository<User> userRepository,
            IEncryptionService encryptionService) : base(unitOfWork, userQuery, authenticatedUser)
        {
            _tokenService = tokenService;
            _userRepository = userRepository;
            _encryptionService = encryptionService;
        }

        protected override async Task<ICommandResult<LoginUserResponse>> HandleRequest(AcceptNewUserTermsCommand command, CancellationToken cancellationToken)
        {
            if (!command.IsValid())
                return new CommandResult<LoginUserResponse>(false, "Erros", command.Notifications);

            var user = await _userQuery.GetById(command.UserId.ToEntityId());

            user.AcceptNewTerms(command);

            user.Login();

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
                User = user
            };

            await _userRepository.UpdateAsync(user);

            return new CommandResult<LoginUserResponse>(true, "Success", response);
        }
    }
}
using Alpha.Domain.Entities;
using Alpha.Domain.Queries.Users;
using Alpha.Domain.Responses.Users;
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

namespace Alpha.Domain.Commands.Users
{
    public class ActivateUserCommand : OctaNotifiable, IRequest<ICommandResult<LoginUserResponse>>
    {
        public string UserId { get; set; }
        public string ActivationCode { get; set; }
        public string DocumentNumber { get; set; }
        public DateTime? Birthdate { get; set; }
        public string SaltPassword { get; set; }
        public string ActivationIp { get; set; }
        public string ActivationBrowser { get; set; }
        public string ActivationOperationalSystem { get; set; }

        public bool IsValid()
        {
            ValidationResult = new ActivateUserCommandValidator().Validate(this);
            NotifyValidationErros();
            return ValidationResult.IsValid;
        }
    }

    public class ActivateUserRequestHandler : CommandHandlerBase<User, ActivateUserCommand, ICommandResult<LoginUserResponse>>
    {
        private readonly IAuthService _tokenService;
        private readonly IRepository<User> _userRepository;
        private readonly IEncryptionService _encryptionService;

        public ActivateUserRequestHandler(
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

        protected override async Task<ICommandResult<LoginUserResponse>> HandleRequest(ActivateUserCommand command, CancellationToken cancellationToken)
        {
            if (!command.IsValid())
                return new CommandResult<LoginUserResponse>(false, "Errors", command.Notifications);

            var user = await _userQuery.GetById(command.UserId.ToEntityId());
            string userEncryptedPassword = user.Password;

            if (command.SaltPassword != null)
            {
                var userEncryptedPasswordResponse = _encryptionService.GenerateEncryptedPassword(command.SaltPassword);

                if (!userEncryptedPasswordResponse.IsSuccess)
                {
                    command.AddNotification("Erro", "Não foi possivel utilizar a senha informada para o usuário");
                    return new CommandResult<LoginUserResponse>(false, "Errors", command.Notifications);
                }

                userEncryptedPassword = _encryptionService.Encrypt(userEncryptedPasswordResponse.Password);
            }

            if ((user.EmailActivationCode == command.ActivationCode.Trim() && (!user.EmailActivationCodeExpirationDate.HasValue || user.EmailActivationCodeExpirationDate.Value < DateTime.UtcNow.ToLocalTimeZone())) ||
                (user.WhatsappActivationCode == command.ActivationCode.Trim() && (!user.WhatsappActivationCodeExpirationDate.HasValue || user.WhatsappActivationCodeExpirationDate.Value < DateTime.UtcNow.ToLocalTimeZone())) ||
                (user.CellPhoneActivationCode == command.ActivationCode.Trim() && (!user.CellPhoneActivationCodeExpirationDate.HasValue || user.CellPhoneActivationCodeExpirationDate.Value < DateTime.UtcNow.ToLocalTimeZone())))
            {
                command.AddNotification("Erro", "Código de Ativação está expirado");
                return new CommandResult<LoginUserResponse>(false, "Errors", command.Notifications);
            }

            if (user.EmailActivationCode == command.ActivationCode.Trim()) user.VerifyEmail();

            else if (user.WhatsappActivationCode == command.ActivationCode.Trim())
            {
                user.VerifyWhatsapp();
                user.VerifyCellphone();
            }

            else if (user.CellPhoneActivationCode == command.ActivationCode.Trim()) user.VerifyCellphone();

            else
            {
                command.AddNotification("Erro", "Código de Ativação é inválido");
                return new CommandResult<LoginUserResponse>(false, "Errors", command.Notifications);
            }

            user.Activate(command, userEncryptedPassword, user.EncryptedDocumentNumber);

            user.Login();

            bool? isExpirationDate;
            bool isApprovedNotificationViewed;

            SetUserLogin(out isExpirationDate,
                   out isApprovedNotificationViewed);

            var tokenRequest = new CreateTokenRequest()
            {
                Email = user.Email ?? user.CellPhone,
                Id = user.Id.ToString(),
                Name = $"{user.FirstName} {user.LastName}",
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
                IsNewNotification = user.IsNewNotification,
                IsApprovedNotificationViewed = isApprovedNotificationViewed,
            };
            
            await _userRepository.UpdateAsync(user);

            return new CommandResult<LoginUserResponse>(true, "Success", response);
        }

        private static void SetUserLogin(out bool? isExpirationDate,
           out bool isApprovedNotificationViewed)
        {      
            isApprovedNotificationViewed = false;
            isExpirationDate = null;            
        }
    }
}

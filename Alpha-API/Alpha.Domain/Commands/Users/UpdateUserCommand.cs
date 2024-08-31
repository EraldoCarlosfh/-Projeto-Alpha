using Alpha.Domain.Entities;
using Alpha.Domain.Queries.Users;
using Alpha.Domain.Validators.Commands.Users;
using Alpha.Framework.MediatR.Data.Repositories;
using Alpha.Framework.MediatR.Data.UnitOfWork;
using Alpha.Framework.MediatR.EventSourcing.Responses;
using Alpha.Framework.MediatR.Notifications;
using Alpha.Framework.MediatR.Resources.Extensions;
using Alpha.Framework.MediatR.SecurityService.DataTransferObjects;
using Alpha.Framework.MediatR.SecurityService.Models;
using Alpha.Framework.MediatR.SecurityService;
using Alpha.Integrations.Encryption;
using MediatR;
using Alpha.Framework.MediatR.EventSourcing.Validators.Attributes;
using System.Threading.Tasks;
using System.Threading;
using System;

namespace Alpha.Domain.Commands.Users
{
    public class UpdateUserCommand : OctaNotifiable, IRequest<ICommandResult<EmptyResponse>>
    {
        [FullName]
        public string FullName { get; set; }
        public string CellPhone { get; set; }
        public string Email { get; set; }
        public string DocumentNumber { get; set; }
        public string Id { get; set; }
        public DateTime Birthdate { get; set; }

        public bool IsValid()
        {
            ValidationResult = new UpdateUserCommandValidator().Validate(this);
            NotifyValidationErros();
            return ValidationResult.IsValid;
        }
    }

    public class UpdateUserRequestHandler : CommandHandlerBase<User, UpdateUserCommand, ICommandResult<EmptyResponse>>
    {
        private readonly IRepository<User> _userRepository;
        private readonly IMediator _mediator;
        private readonly IAuthService _authenticationService;
        private readonly IEncryptionService _encryptionService;

        public UpdateUserRequestHandler(
            IUnitOfWork unitOfWork,
            AuthenticatedUserModel authenticatedUser,
            IAuthService authenticationService,
            IUserQuery userQuery,
            IEncryptionService encryptionService,
            IRepository<User> userRepository,
            IMediator mediator) : base(unitOfWork, userQuery, authenticatedUser)
        {
            _userRepository = userRepository;
            _mediator = mediator;
            _authenticationService = authenticationService;
            _encryptionService = encryptionService;
        }

        protected override async Task<ICommandResult<EmptyResponse>> HandleRequest(UpdateUserCommand command, CancellationToken cancellationToken)
        {
            if (!command.IsValid())
                return ValidationErrorsForEmptyResponse(command.Notifications);

            var user = await _userQuery.GetById(command.Id.ToEntityId());

            ValidateCommunicationChannels(command, user);

            user.UpdateFullname(command.FullName);

            user.Update(command.CellPhone, command.Birthdate, command.Email);

            user.UpdateDocumentNumber(command.DocumentNumber, _encryptionService.Encrypt(command.DocumentNumber));

            await _userRepository.UpdateAsync(user);

            return Success();
        }

        private void ValidateCommunicationChannels(UpdateUserCommand command, User user)
        {
            var codeResponseWhatsapp = new GenerateVerificationCodeResponse();
            var codeResponseEmail = new GenerateVerificationCodeResponse();
            if (command.CellPhone.IsNullOrEmpty() && (user.CellPhone != command.CellPhone || !user.IsCellPhoneVerified))
            {
                codeResponseWhatsapp = _authenticationService.GenerateVerificationCode(command.CellPhone.ToLower());

                user.UpdateCellPhone(command.CellPhone, codeResponseWhatsapp.Code, codeResponseWhatsapp.ExpirationDate);
            }

            if (command.Email.IsNullOrEmpty() && (user.Email != command.Email || !user.IsEmailVerified))
            {
                codeResponseEmail = _authenticationService.GenerateVerificationCode(command.Email.ToLower());

                user.UpdateEmail(command.Email, codeResponseEmail.Code, codeResponseEmail.ExpirationDate);
            }
        }
    }
}
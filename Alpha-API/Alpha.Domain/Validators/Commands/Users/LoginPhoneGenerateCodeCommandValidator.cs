using Alpha.Domain.Commands.Users;
using Alpha.Domain.Extensions.Commons;
using Alpha.Framework.MediatR.EventSourcing.Commands;
using FluentValidation;
using Alpha.Domain.Validators;

namespace Alpha.Domain.Validators.Commands.Users
{
    public class LoginPhoneGenerateCodeCommandValidator : CommandValidator<LoginPhoneGenerateCodeCommand>
    {
        public LoginPhoneGenerateCodeCommandValidator()
        {
            RuleFor(x => x.PhoneNumber).NotEmpty().WithMessage(ValidationMessages.RequiredField.Formater("Número de celular"));
        }
    }
}

using Alpha.Domain.Commands.Users;
using Alpha.Framework.MediatR.EventSourcing.Commands;
using FluentValidation;

namespace Alpha.Domain.Validators.Commands.Products
{
    public class DeleteProductCommandValidator : CommandValidator<DeleteProductCommand>
    {
        public DeleteProductCommandValidator() : base()
        {
            RuleFor(x => x.ProductId).NotEmpty().WithMessage("Id do produto deve ser informado.");
        }
    }
}

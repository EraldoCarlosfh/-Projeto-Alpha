using Alpha.Domain.Commands.Users;
using Alpha.Framework.MediatR.EventSourcing.Commands;
using FluentValidation;

namespace Alpha.Domain.Validators.Commands.Products
{
    public class UpdateProductCommandValidator : CommandValidator<UpdateProductCommand>
    {
        public UpdateProductCommandValidator() : base()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Nome do produto deve ser informado.");
            RuleFor(x => x.BarCode).NotEmpty().WithMessage("Código de Barras do produto deve ser informado.");
            RuleFor(x => x.Image).NotEmpty().WithMessage("Imagem do produto deve ser informada.");
            RuleFor(x => x.Price).NotEmpty().WithMessage("Preço do produto deve ser informado.");
        }
    }
}

using Alpha.Domain.Entities;
using Alpha.Framework.MediatR.EventSourcing.Commands;
using FluentValidation;

namespace Alpha.Domain.Validators.Entities.Products
{
    public class ProductEntityValidator : CommandValidator<Product>
    {
        public ProductEntityValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Produto não foi encontrado");
            When(x => x.Id != null, () =>
            {
                RuleFor(x => x.Name).NotEmpty().WithMessage("Nome do produto deve ser informado.");
            });
        }
    }
}

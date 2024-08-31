using Alpha.Domain.Entities;
using Alpha.Framework.MediatR.Data.Repositories;
using Alpha.Framework.MediatR.Data.UnitOfWork;
using Alpha.Framework.MediatR.Notifications;
using Alpha.Framework.MediatR.SecurityService.Models;
using MediatR;
using Alpha.Domain.Queries.Users;
using Alpha.Domain.Validators.Commands.Products;
using Alpha.Framework.MediatR.Resources.Extensions;

namespace Alpha.Domain.Commands.Users
{
    public class DeleteProductCommand : OctaNotifiable, IRequest<ICommandResult<Product>>
    {
        public string ProductId { get; set; }

        public bool IsValid()
        {
            ValidationResult = new DeleteProductCommandValidator().Validate(this);
            NotifyValidationErros();
            return ValidationResult.IsValid;
        }
    }

    public class DeleteProductRequestHandler : CommandHandlerBase<Product, DeleteProductCommand, ICommandResult<Product>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IRepository<Product> _productRepository;

        public DeleteProductRequestHandler(
            IUnitOfWork uow,
            IUnitOfWork unitOfWork,
            AuthenticatedUserModel authenticatedUser,
            IRepository<Product> productRepository,
            IUserQuery userQuery) : base(unitOfWork, userQuery, authenticatedUser)
        {
            _uow = uow;
            _productRepository = productRepository;
        }

        protected override async Task<ICommandResult<Product>> HandleRequest(DeleteProductCommand command, CancellationToken cancellationToken)
        {
            if (!command.IsValid())
                return ValidationErrors(command.Notifications);

            var product = await _productRepository.GetById(command.ProductId.ToEntityId());

            product.UpdateActivated(false);

            await _productRepository.UpdateAsync(product);

            return Success(product);
        }
    }
}
using Alpha.Domain.Entities;
using Alpha.Framework.MediatR.Data.Repositories;
using Alpha.Framework.MediatR.Data.UnitOfWork;
using Alpha.Framework.MediatR.Notifications;
using Alpha.Framework.MediatR.SecurityService.Models;
using MediatR;
using Alpha.Domain.Queries.Users;
using Alpha.Domain.Validators.Commands.Products;
using Alpha.Framework.MediatR.Resources.Extensions;
using System.Threading.Tasks;
using System.Threading;

namespace Alpha.Domain.Commands.Users
{
    public class UpdateProductCommand : OctaNotifiable, IRequest<ICommandResult<Product>>
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string BarCode { get; set; }
        public string Image { get; set; }
        public decimal? Price { get; set; }

        public bool IsValid()
        {
            ValidationResult = new UpdateProductCommandValidator().Validate(this);
            NotifyValidationErros();
            return ValidationResult.IsValid;
        }
    }

    public class UpdateProductRequestHandler : CommandHandlerBase<Product, UpdateProductCommand, ICommandResult<Product>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IRepository<Product> _productRepository;

        public UpdateProductRequestHandler(
            IUnitOfWork uow,
            IUnitOfWork unitOfWork,
            AuthenticatedUserModel authenticatedUser,
            IRepository<Product> productRepository,
            IUserQuery userQuery) : base(unitOfWork, userQuery, authenticatedUser)
        {
            _uow = uow;
            _productRepository = productRepository;
        }

        protected override async Task<ICommandResult<Product>> HandleRequest(UpdateProductCommand command, CancellationToken cancellationToken)
        {
            if (!command.IsValid())
                return ValidationErrors(command.Notifications);

            var product = await _productRepository.GetById(command.Id.ToEntityId());

            product.Update(command);

            await _productRepository.UpdateAsync(product);

            return Success(product);
        }
    }
}
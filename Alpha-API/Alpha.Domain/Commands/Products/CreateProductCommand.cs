using Alpha.Domain.Entities;
using Alpha.Framework.MediatR.Data.Repositories;
using Alpha.Framework.MediatR.Data.UnitOfWork;
using Alpha.Framework.MediatR.Notifications;
using Alpha.Framework.MediatR.SecurityService.Models;
using MediatR;
using Alpha.Domain.Queries.Users;
using Alpha.Domain.Validators.Commands.Products;
using System.Threading.Tasks;
using System.Threading;

namespace Alpha.Domain.Commands.Users
{
    public class CreateProductCommand : OctaNotifiable, IRequest<ICommandResult<Product>>
    {
        public string Name { get; set; }
        public string BarCode { get; set; }
        public string Image { get; set; }
        public decimal? Price { get; set; }

        public bool IsValid()
        {
            ValidationResult = new CreateProductCommandValidator().Validate(this);
            NotifyValidationErros();
            return ValidationResult.IsValid;
        }
    }

    public class CreateProductRequestHandler : CommandHandlerBase<Product, CreateProductCommand, ICommandResult<Product>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IRepository<Product> _productRepository;

        public CreateProductRequestHandler(
            IUnitOfWork uow,
            IUnitOfWork unitOfWork,
            AuthenticatedUserModel authenticatedUser,
            IRepository<Product> productRepository,
            IUserQuery userQuery) : base(unitOfWork, userQuery, authenticatedUser)
        {
            _uow = uow;
            _productRepository = productRepository;
        }

        protected override async Task<ICommandResult<Product>> HandleRequest(CreateProductCommand command, CancellationToken cancellationToken)
        {
            if (!command.IsValid())
                return ValidationErrors(command.Notifications);

            var product = Product.Create(command);

            await _productRepository.AddAsync(product);

            return Success(product);
        }
    }
}
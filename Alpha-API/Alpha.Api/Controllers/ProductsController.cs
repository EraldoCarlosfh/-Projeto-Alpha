using Alpha.Api.ViewModels.Products;
using Alpha.Domain.Commands.Users;
using Alpha.Domain.Entities;
using Alpha.Domain.Queries.Products;
using Alpha.Framework.MediatR.Api.Authorizations;
using Alpha.Framework.MediatR.EventSourcing.Domains;
using Alpha.Framework.MediatR.Resources.Extensions;
using Alpha.Framework.MediatR.SecurityService.Models;
using Alpha.Integrations.Encryption;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Alpha.Api.Controllers
{
    [Route("api/products")]
    [ApiController]
    [CustomAuthorization]
    public class ProductsController : AlphaBaseController
    {
        private readonly IMediator _mediator;
        private readonly IProductQuery _productQuery;
        private readonly IEncryptionService _encryptionService;
        private readonly AuthenticatedUserModel _authenticatedUserModel;

        public ProductsController(
            IMediator mediator,
            IMapper mapper,
            IProductQuery productQuery,
            IEncryptionService encryptionService,
            AuthenticatedUserModel authenticatedUserModel) : base(mapper)
        {
            _mediator = mediator;
            _productQuery = productQuery;
            _encryptionService = encryptionService;
            _authenticatedUserModel = authenticatedUserModel;
        }

        [HttpGet]
        [Route("{productId}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ProductViewModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetProductByProductId(string productId)
        {
            var response = await _productQuery.GetById(productId.ToEntityId());
            if (response == null) return NotFound($"O produto com o id {productId} não foi encontrado");

            var viewModel = _mapper.Map<ProductViewModel>(response);

            return CustomResponse(viewModel);
        }

        [HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(List<ProductViewModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetAllProducts()
        {
            var response = await _productQuery.GetAll();
            if (response == null) return NotFound();

            return CustomResponse<List<Product>, List<ProductViewModel>>(response);
        }

        [HttpGet]
        [Route("all-page")]
        [ProducesResponseType(typeof(List<ProductViewModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetAllProductsPage([FromQuery] PagedSearchRequest request)
        {
            var response = await _productQuery.ListPageProducts(request);
            if (response == null) return NotFound();

            return CustomPageResponse<List<ProductViewModel>>(response);
        }

        [HttpPost]
        [Route("new-product")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ProductViewModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateNewProduct(CreateProductCommand request)
        {
            var response = await _mediator.Send(request);

            return CustomResponse<Product, ProductViewModel>(response);
        }       

        [HttpPut]
        [Route("update")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateUser(UpdateProductCommand request)
        {
            var response = await _mediator.Send(request);

            return CustomResponse(response);
        } 

        

        [HttpDelete]
        [Route("{productId}")]
        [ProducesResponseType(typeof(object), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> DeleteProductByProductId(string productId)
        {
            var command = new DeleteProductCommand
            {
                ProductId = productId
            };

            var response = await _mediator.Send(command);
            return CustomResponse(response);
        }       
    }
}

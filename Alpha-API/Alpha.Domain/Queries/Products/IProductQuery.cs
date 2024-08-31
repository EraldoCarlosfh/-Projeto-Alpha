using Alpha.Domain.Entities;
using Alpha.Framework.MediatR.Data.Queries;
using Alpha.Framework.MediatR.EventSourcing.Domains;
using Alpha.Framework.MediatR.EventSourcing.Entity;

namespace Alpha.Domain.Queries.Products
{
    public interface IProductQuery : IQuery<Product>
    {
        Task<Product> GeProductByEntityId(EntityId id);
        Task<PagedSearchResult> ListPageProducts(PagedSearchRequest request);
        Task<Product> GetProductByNome(string name);

    }
}

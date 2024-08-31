using Alpha.Data;
using Alpha.Domain.Entities;
using Alpha.Framework.MediatR.EventSourcing.Domains;
using Alpha.Framework.MediatR.EventSourcing.Entity;
using Alpha.Framework.MediatR.Resources.Extensions;
using Alpha.Framework.MediatR.SecurityService.Models;
using Alpha.Integrations.Encryption;
using Alpha.Domain.Queries.Products;
using System.Data;
using System.Data.Entity;
using Alpha.Framework.MediatR.Data.Queries;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace Alpha.Query.Queries.Users
{
    namespace OctaTech.Query.Queries.Users
    {
        public class ProductQuery : IProductQuery
        {
            protected AlphaDataContext _dataContext;
            private readonly IEncryptionService _encryptionService;
            private readonly AuthenticatedUserModel _autheticatadUser;

            public ProductQuery(
                AlphaDataContext dbContext,
                IEncryptionService encryptionService,
                AuthenticatedUserModel autheticatadUser)
            {
                _dataContext = dbContext;
                _autheticatadUser = autheticatadUser;
                _encryptionService = encryptionService;
            }

            #region Base

            public void Dispose()
            {
                _dataContext.Dispose();
            }

            public async Task<Product> GetById(EntityId id)
            {
                return await _dataContext.Products
                    .FirstOrDefaultAsync(c => c.Id == id);
            }

            public async Task<List<Product>> GetAll()
            {
                return await _dataContext.Products
                    .ToListAsync();
            }

            public async Task<List<Product>> GetActives()
            {
                return await _dataContext.Products
                    .Where(c => c.IsActive).ToListAsync();
            }

            #endregion

            public async Task<Product> GeProductByEntityId(EntityId id)
            {
                return await _dataContext.Products
                    .FirstOrDefaultAsync(x => x.Id == id && x.IsActive);
            }

            public async Task<PagedSearchResult> ListPageProducts(PagedSearchRequest request)
            {
                var globarFilter = request?.GlobalFilter?.ToLower();

                var query = _dataContext.Products.Where(x => x.IsActive);

                if (!globarFilter.IsNullOrEmpty())
                    query = query.Where(c => globarFilter.Contains(c.Name.ToLower()));

                query = FilterByPerson(query, request);

                return await query.Page(request);
            }

            private IQueryable<Product> FilterByPerson(IQueryable<Product> query, PagedSearchRequest request)
            {        
                if (request.Order == OrderingOption.Ascending)
                    query = query.OrderBy(c => c.Name);

                if (request.Order == OrderingOption.Descending)
                    query = query.OrderByDescending(c => c.Name);

                if (request.Order == OrderingOption.LowerValue)
                    query = query.OrderBy(c => c.Price);

                if (request.Order == OrderingOption.MoreValue)
                    query = query.OrderByDescending(c => c.Price);

                return query;
            }

            public async Task<Product> GetProductByNome(string name)
            {
                return await _dataContext.Products
                    .FirstOrDefaultAsync(c => c.Name == name && c.IsActive);
            }
        }
    }
}
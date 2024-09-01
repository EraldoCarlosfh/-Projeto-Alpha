using Alpha.Framework.MediatR.EventSourcing.Domains;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Alpha.Framework.MediatR.Data.Queries
{
    public static class PagingExtensions
    {
        public static async Task<PagedSearchResult> Page(this IQueryable<object> list, PagedSearchRequest pagedSearchRequest)
        {
            try
            {
                int count = list.Count();

                pagedSearchRequest.PageSize = pagedSearchRequest.PageSize > 0 ? pagedSearchRequest.PageSize : 10;

                list = list
                    .Skip(pagedSearchRequest.PageIndex * pagedSearchRequest.PageSize)
                    .Take(pagedSearchRequest.PageSize);


                var pagedSearchResult = new PagedSearchResult()
                {
                    SearchResult = list.AsNoTracking().ToList(),
                    TotalRecords = count,
                    PageIndex = pagedSearchRequest.PageIndex,
                    PageCount = count / pagedSearchRequest.PageSize,
                    PageSize = pagedSearchRequest.PageSize
                };

                return pagedSearchResult;
            }
            catch (Exception ex)
            {
                var pagedSearchResult = new PagedSearchResult()
                {
                    SearchResult = null,
                    TotalRecords = 0,
                    PageIndex = pagedSearchRequest.PageIndex,
                    PageCount = 0,
                    PageSize = pagedSearchRequest.PageSize
                };

                return pagedSearchResult;
            }
        }

        public static async Task<PagedSearchResult> PageResult(this IQueryable<object> list, PagedSearchRequest pagedSearchRequest)
        {
            try
            {
                var pagedSearchResult = new PagedSearchResult()
                {
                    SearchResult = list.AsNoTracking().ToList(),
                    TotalRecords = list.Count(),
                    PageIndex = pagedSearchRequest.PageIndex,
                    PageCount = list.Count() / pagedSearchRequest.PageSize,
                    PageSize = pagedSearchRequest.PageSize
                };

                return pagedSearchResult;
            }
            catch (Exception ex)
            {
                var pagedSearchResult = new PagedSearchResult()
                {
                    SearchResult = null,
                    TotalRecords = 0,
                    PageIndex = pagedSearchRequest.PageIndex,
                    PageCount = 0,
                    PageSize = pagedSearchRequest.PageSize
                };

                return pagedSearchResult;
            }
        }
    }
}

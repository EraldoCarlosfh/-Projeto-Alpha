using System.Collections.Generic;

namespace Alpha.Framework.MediatR.EventSourcing.Domains
{
    public class PagedResult<T> where T : class
    {
        public List<T> SearchResult { get; set; }
        public int PageSize { get; set; }
        public int PageCount { get; set; }
        public int PageIndex { get; set; }
        public int TotalRecords { get; set; }
    }
}

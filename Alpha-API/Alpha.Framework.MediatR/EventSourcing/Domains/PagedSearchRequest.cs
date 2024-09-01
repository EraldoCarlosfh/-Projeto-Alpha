using System;
using System.Collections.Generic;
namespace Alpha.Framework.MediatR.EventSourcing.Domains
{
    public class PagedSearchRequest
    {
        public string GlobalFilter { get; set; }
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        public OrderingOption Order { get; set; }
    }

    public enum OrderingOption
    {
        Ascending = 1,
        Descending = 2,
        MoreValue = 3,
        LowerValue = 4
    }
}

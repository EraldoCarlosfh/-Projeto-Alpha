﻿namespace Alpha.Framework.MediatR.EventSourcing.Domains
{
    public class PagedSearchResult
    {
        public List<object> SearchResult { get; set; }
        public int PageSize { get; set; }
        public int PageCount { get; set; }
        public int PageIndex { get; set; }
        public int TotalRecords { get; set; }
    }
}
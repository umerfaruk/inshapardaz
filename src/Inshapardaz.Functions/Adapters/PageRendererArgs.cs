﻿using System;
using Inshapardaz.Domain.Entities;
using Inshapardaz.Functions.Views;

namespace Inshapardaz.Functions.Adapters
{
    public class PageRendererArgs<T>
    {
        public Page<T> Page { get; set; }

        public PagedRouteArgs RouteArguments { get; set; }

        public Func<int, int, string, LinkView> LinkFunc { get; set; }

        public Func<int, int, int, string, LinkView> LinkFuncWithParameter { get; set; }
    }

    public class PagedRouteArgs
    {
        public int PageNumber { get; set; }

        public int PageSize { get; set; }
    }

    public class DictionaryPagedRouteArgs : PagedRouteArgs
    {
        public int DictionaryId { get; set; }
    }

    public class DictionarySearchPageRouteArgs : DictionaryPagedRouteArgs
    {
        public int Id { get; set; }

        public string Query { get; set; }
    }

    public class RouteWithTitlePageRouteArgs : PagedRouteArgs
    {
        public string Title { get; set; }
    }
}
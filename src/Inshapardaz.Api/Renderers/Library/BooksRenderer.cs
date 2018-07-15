﻿using System.Collections.Generic;
using System.Linq;
using Inshapardaz.Api.View;
using Inshapardaz.Api.View.Library;
using Inshapardaz.Domain.Entities.Library;
using Inshapardaz.Domain.Helpers;

namespace Inshapardaz.Api.Renderers.Library
{
    public interface IRenderBooks
    {
        PageView<BookView> Render(PageRendererArgs<Book> source);
    }

    public class BooksRenderer : IRenderBooks
    {
        private readonly IRenderBook _authorRenderer;
        private readonly IRenderLink _linkRenderer;
        private readonly IUserHelper _userHelper;

        public BooksRenderer(IRenderLink linkRenderer, IUserHelper userHelper, IRenderBook authorRenderer)
        {
            _authorRenderer = authorRenderer;
            _linkRenderer = linkRenderer;
            _userHelper = userHelper;
        }

        public PageView<BookView> Render(PageRendererArgs<Book> source)
        {
            var page = new PageView<BookView>(source.Page.TotalCount, source.Page.PageSize, source.Page.PageNumber)
            {
                Data = source.Page.Data?.Select(x => _authorRenderer.Render(x))
            };

            var links = new List<LinkView>
            {
                _linkRenderer.Render(source.RouteName, RelTypes.Self,
                                     CreateRouteParameters(source, page.CurrentPageIndex, page.PageSize))
            };

            if (_userHelper.IsContributor)
            {
                links.Add(_linkRenderer.Render("CreateBook", RelTypes.Create));
            }

            if (page.CurrentPageIndex < page.PageCount)
            {
                links.Add(_linkRenderer.Render(source.RouteName, RelTypes.Next,
                                               CreateRouteParameters(source, page.CurrentPageIndex + 1, page.PageSize)));
            }

            if (page.PageCount > 1 && page.CurrentPageIndex > 1)
            {
                links.Add(_linkRenderer.Render(source.RouteName, RelTypes.Previous,
                                               CreateRouteParameters(source, page.CurrentPageIndex - 1, page.PageSize)));
            }

            page.Links = links;
            return page;
        }

        private object CreateRouteParameters(PageRendererArgs<Book> source, int pageNumber, int pageSize)
        {
            var args = source.RouteArguments ?? new PagedRouteArgs();
            args.PageNumber = pageNumber;
            args.PageSize = pageSize;
            return args;
        }
    }
}
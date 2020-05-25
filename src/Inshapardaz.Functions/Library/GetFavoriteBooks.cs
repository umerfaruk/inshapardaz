using Inshapardaz.Domain.Models.Library;
using Inshapardaz.Domain.Ports.Library;
using Inshapardaz.Functions.Authentication;
using Inshapardaz.Functions.Converters;
using Inshapardaz.Functions.Views;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Paramore.Darker;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Inshapardaz.Functions.Library.Books
{
    public class GetFavoriteBooks : QueryBase
    {
        public GetFavoriteBooks(IQueryProcessor queryProcessor)
        : base(queryProcessor)
        {
        }

        [FunctionName("GetFavoriteBooks")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "library/{libraryId}/favorites")] HttpRequest req,
            int libraryId,
            [AccessToken] ClaimsPrincipal principal,
            CancellationToken token)
        {
            if (principal == null)
            {
                return new UnauthorizedResult();
            }

            var pageNumber = GetQueryParameter(req, "pageNumber", 1);
            var pageSize = GetQueryParameter(req, "pageSize", 10);

            var query = new GetFavoriteBooksQuery(libraryId, principal.GetUserId(), pageNumber, pageSize);
            var books = await QueryProcessor.ExecuteAsync(query, cancellationToken: token);

            var args = new PageRendererArgs<BookModel>
            {
                Page = books,
                RouteArguments = new PagedRouteArgs { PageNumber = pageNumber, PageSize = pageSize },
                LinkFunc = Link
            };

            return new OkObjectResult(args.Render(principal));
        }

        public static LinkView Link(int libraryId, string relType = RelTypes.Self) => SelfLink($"library/{libraryId}/favorites", relType);

        public static LinkView Link(int pageNumber = 1, int pageSize = 10, string relType = RelTypes.Self)
            => SelfLink("books/favorite", relType, queryString: new Dictionary<string, string>
            {
                { "pageNumber", pageNumber.ToString()},
                { "pageSize", pageSize.ToString()}
            });
    }
}
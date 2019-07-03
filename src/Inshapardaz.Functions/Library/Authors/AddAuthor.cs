using System.Threading;
using System.Threading.Tasks;
using Inshapardaz.Domain.Ports.Library;
using Inshapardaz.Functions.Adapters.Library;
using Inshapardaz.Functions.Authentication;
using Inshapardaz.Functions.Extensions;
using Inshapardaz.Functions.Views;
using Inshapardaz.Functions.Views.Library;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Paramore.Brighter;

namespace Inshapardaz.Functions.Library.Authors
{
    public class AddAuthor : FunctionBase
    {
        private readonly IRenderAuthor _authorRenderer;
        public AddAuthor(IAmACommandProcessor commandProcessor, IFunctionAppAuthenticator authenticator, IRenderAuthor authorRenderer)
        : base(commandProcessor, authenticator)
        {
            _authorRenderer = authorRenderer;
        }

        [FunctionName("AddAuthor")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "authors")] HttpRequest req,
            ILogger log, CancellationToken token)
        {
            var auth = await AuthenticateAsWriter(req, log);
            var category = await ReadBody<AuthorView>(req);

            var request = new AddAuthorRequest(category.Map());
            await CommandProcessor.SendAsync(request, cancellationToken: token);

            var renderResult = _authorRenderer.Render(auth.User, request.Result);
            return new CreatedResult(renderResult.Links.Self(), renderResult);
        }

        public static LinkView Link(string relType = RelTypes.Self) => SelfLink("authors", relType, "POST");
    }
}

using System.Threading;
using System.Threading.Tasks;
using Inshapardaz.Domain.Ports.Library;
using Inshapardaz.Functions.Adapters.Library;
using Inshapardaz.Functions.Authentication;
using Inshapardaz.Functions.Views;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Paramore.Brighter;

namespace Inshapardaz.Functions.Library.Books
{
    public class GetBookById : FunctionBase
    {
        private readonly IRenderBook _bookRenderer;
        public GetBookById(IAmACommandProcessor commandProcessor, IFunctionAppAuthenticator authenticator, IRenderBook bookRenderer)
        : base(commandProcessor, authenticator)
        {
            _bookRenderer = bookRenderer;
        }

        [FunctionName("GetBookById")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "books/{bookId}")] HttpRequest req,
            ILogger log, int bookId, CancellationToken token)
        {
            var auth = await TryAuthenticate(req, log);

            var request = new GetBookByIdRequest(bookId);
            await CommandProcessor.SendAsync(request, cancellationToken: token);

            return new OkObjectResult(_bookRenderer.Render(auth?.User, request.Result));
        }

        public static LinkView Link(int bookId, string relType = RelTypes.Self) => SelfLink($"books/{bookId}", relType);
    }
}

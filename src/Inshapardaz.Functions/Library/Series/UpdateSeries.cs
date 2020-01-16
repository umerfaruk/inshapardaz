using System.IO;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Inshapardaz.Domain.Ports.Library;
using Inshapardaz.Functions.Authentication;
using Inshapardaz.Functions.Converters;
using Inshapardaz.Functions.Extensions;
using Inshapardaz.Functions.Views;
using Inshapardaz.Functions.Views.Library;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Paramore.Brighter;

namespace Inshapardaz.Functions.Library.Series
{
    public class UpdateSeries : CommandBase
    {
        public UpdateSeries(IAmACommandProcessor commandProcessor)
        : base(commandProcessor)
        {
        }

        [FunctionName("UpdateSeries")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "series/{seriesId:int}")] HttpRequest req,
            int seriesId, 
            [AccessToken] ClaimsPrincipal principal, 
            CancellationToken token)
        {

            if (principal == null)
            {
                return new UnauthorizedResult();
            }

            if (!principal.IsWriter())
            {
                return new ForbidResult("Bearer");
            }

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var series = JsonConvert.DeserializeObject<SeriesView>(requestBody);

            series.Id = seriesId;
            var request = new UpdateSeriesRequest(series.Map());
            await CommandProcessor.SendAsync(request, cancellationToken: token);

            var renderResult = request.Result.Series.Render(principal);

            if (request.Result.HasAddedNew)
            {
                return new CreatedResult(renderResult.Links.Self(), renderResult);
            }
            else
            {
                return new OkObjectResult(renderResult);
            }
        }

        public static LinkView Link(int seriesId, string relType = RelTypes.Self) => SelfLink($"series/{seriesId}", relType, "PUT");
    }
}

﻿using System.Threading.Tasks;
using Inshapardaz.Functions.Views;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Paramore.Brighter;

namespace Inshapardaz.Functions.Dictionaries.Words.Translations
{
    public class GetTranslationById : CommandBase
    {
        public GetTranslationById(IAmACommandProcessor commandProcessor)
            : base(commandProcessor)
        {
        }

        [FunctionName("GetTranslationById")]
        public IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "dictionaries/{dictionaryId:int}/words/{wordId:long}/translations/{translationId:int}")] HttpRequest req,
            int dictionaryId, long wordId, int translationId,
            ILogger log)
        {
            return new OkObjectResult($"GET:GetTranslationById({dictionaryId}, {wordId}, {translationId})");
        }
    }
}
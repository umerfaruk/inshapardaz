﻿using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Inshapardaz.Api.View;
using Inshapardaz.Api.Adapters.LanguageUtility;
using Paramore.Brighter;

namespace Inshapardaz.Api.Controllers
{
    public class UtilitiesController : Controller
    {
        private readonly IAmACommandProcessor _commandProcessor;

        public UtilitiesController(IAmACommandProcessor queryProcessor)
        {
            _commandProcessor = queryProcessor;
        }

        [HttpGet("api/alternates/{word}", Name = "GetWordAlternatives")]
        public async Task<IActionResult> Get(string word, int pageNumber = 1, int pageSize = 10)
        {
            if (string.IsNullOrWhiteSpace(word))
            {
                return NotFound();
            }

            var request = new ThesaususRequest
            {
                PageSize = pageSize,
                PageNumber = pageNumber,
                Word = word
            };

            await _commandProcessor.SendAsync(request);
            
            return new ObjectResult(request.Response);
        }

        [HttpPost("api/check/spells", Name = "CheckSpells")]
        public async Task<IActionResult> CheckSpellings([FromBody]SpellCheckRequestView spellCheckRequest)
        {
            var request = new SpellCheckRequest { Sentence = spellCheckRequest.Sentence };
            await _commandProcessor.SendAsync(request);

            return new ObjectResult(request.Response);
        }
    }
}
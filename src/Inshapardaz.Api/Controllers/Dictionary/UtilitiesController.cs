﻿using System.Threading.Tasks;
using Inshapardaz.Api.Adapters.LanguageUtility;
using Inshapardaz.Api.View;
using Inshapardaz.Api.View.Dictionary;
using Microsoft.AspNetCore.Mvc;
using Paramore.Brighter;

namespace Inshapardaz.Api.Controllers.Dictionary
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

            var request = new ThesaurusRequest
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

        [HttpPost("api/check/grammar", Name = "CheckGrammar")]
        public async Task<IActionResult> CheckGrammar([FromBody]GrammarCheckRequestView grammarCheckRequest)
        {
            var request = new GrammarCheckRequest { Paragraph = grammarCheckRequest.Paragraph };
            await _commandProcessor.SendAsync(request);

            return new ObjectResult(request.Response);
        }

        [HttpPost("api/transpile", Name = "Transpile")]
        public async Task<IActionResult> ConvertScript([FromBody]TranspileRequestView transpileRequest)
        {
            var request = new TranspileRequest
            {
                FromLanguage = transpileRequest.FromLanguage,
                ToLanguage = transpileRequest.ToLanguage,
                Source = transpileRequest.Source
            };
            await _commandProcessor.SendAsync(request);

            return new ObjectResult(request.Response);
        }
    }
}

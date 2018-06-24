using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Inshapardaz.Api.Renderers;
using Inshapardaz.Api.Renderers.Dictionary;
using Inshapardaz.Api.View;
using Inshapardaz.Api.View.Dictionary;
using Inshapardaz.Domain.Entities;
using Inshapardaz.Domain.Entities.Dictionary;
using Inshapardaz.Domain.Queries;
using Inshapardaz.Domain.Queries.Dictionary;
using Paramore.Brighter;
using Paramore.Darker;

namespace Inshapardaz.Api.Adapters.Dictionary
{
    public class GetTranslationForWordLanguageRequest : DictionaryRequest
    {
        public GetTranslationForWordLanguageRequest(int dictionaryId)
            : base(dictionaryId)
        {
        }

        public List<TranslationView> Result { get; set; }

        public long WordId { get; set; }

        public Languages Language { get; set; }
    }

    public class GetTranslationForWordLanguageRequestHandler : RequestHandlerAsync<GetTranslationForWordLanguageRequest>
    {
        private readonly IQueryProcessor _queryProcessor;
        private readonly IRenderTranslation _translationRenderer;

        public GetTranslationForWordLanguageRequestHandler(IQueryProcessor queryProcessor, IRenderTranslation translationRenderer)
        {
            _queryProcessor = queryProcessor;
            _translationRenderer = translationRenderer;
        }

        [DictionaryRequestValidation(1, HandlerTiming.Before)]
        public override async Task<GetTranslationForWordLanguageRequest> HandleAsync(GetTranslationForWordLanguageRequest command, CancellationToken cancellationToken = new CancellationToken())
        {
            var translations = await _queryProcessor.ExecuteAsync(new GetTranslationsByLanguageQuery(command.DictionaryId, command.WordId, command.Language), cancellationToken);

            command.Result = translations.Select(t => _translationRenderer.Render(t, command.DictionaryId)).ToList();
            return await base.HandleAsync(command, cancellationToken);
        }
    }
}
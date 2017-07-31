﻿using System.Collections.Generic;
using Inshapardaz.Api.Helpers;
using Inshapardaz.Api.Model;
using Inshapardaz.Domain.Model;

namespace Inshapardaz.Api.Renderers
{
    public class WordDetailRenderer : RendrerBase,
        IRenderResponseFromObject<WordDetail, WordDetailView>
    {
        private readonly IRenderEnum _enumRenderer;

        private readonly IUserHelper _userHelper;

        public WordDetailRenderer(IRenderLink linkRenderer, IRenderEnum enumRenderer, IUserHelper userHelper)
            : base(linkRenderer)
        {
            _enumRenderer = enumRenderer;
            _userHelper = userHelper;
        }

        public WordDetailView Render(WordDetail source)
        {
            var result = source.Map<WordDetail, WordDetailView>();

            result.Attributes = _enumRenderer.RenderFlags(source.Attributes).Trim(',');
            result.Language = _enumRenderer.Render((Languages)source.Language);
            var links = new List<LinkView>
            {
                LinkRenderer.Render("GetWordDetailsById", "self", new {id = source.WordInstanceId}),
                LinkRenderer.Render("GetWordById", "word", new {id = source.WordInstanceId}),
                LinkRenderer.Render("GetWordTranslationsById", "translations", new {id = source.WordInstanceId}),
                LinkRenderer.Render("GetWordMeaningById", "meanings", new {id = source.WordInstanceId}),
                LinkRenderer.Render("GetWordRelationsById", "relationships", new {id = source.WordInstanceId})
            };

            if (_userHelper.IsContributor)
            {
                links.Add(LinkRenderer.Render("UpdateWordDetail", "update", new { id = source.Id }));
                links.Add(LinkRenderer.Render("DeleteWordDetail", "delete", new { id = source.Id }));
                links.Add(LinkRenderer.Render("AddTranslation", "addTranslation", new { id = source.Id }));
                links.Add(LinkRenderer.Render("AddMeaning", "addMeaning", new { id = source.Id }));
            }

            result.Links = links;
            return result;
        }
    }
}
﻿using System;
using System.Collections.Generic;
using Inshapardaz.Api.Model;
using Inshapardaz.Api.Renderers;
using Inshapardaz.Domain.Model;

namespace Inshapardaz.Api.UnitTests.Fakes.Renderers
{
    public class FakeWordRenderer : IRenderResponseFromObject<Word, WordView>
    {
        private readonly WordView _response = new WordView();
        private readonly List<LinkView> _links = new List<LinkView>();

        public WordView Render(Word source)
        {
            _response.Links = _links;
            return _response;
        }

        public FakeWordRenderer WithLink(string rel, Uri value)
        {
            _links.Add(new LinkView { Rel = rel, Href = value });
            return this;
        }
    }
}
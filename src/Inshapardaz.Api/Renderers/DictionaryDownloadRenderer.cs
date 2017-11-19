﻿using System;
using System.Collections.Generic;
using Inshapardaz.Api.Model;
using Inshapardaz.Api.View;

namespace Inshapardaz.Api.Renderers
{
    public interface IRenderDictionaryDownload
    {
        DownloadDictionaryView Render(DownloadJobModel source);
    }

    public class DictionaryDownloadRenderer : IRenderDictionaryDownload
    {
        private readonly IRenderLink _linkRenderer;

        public DictionaryDownloadRenderer(IRenderLink linkRenderer)
        {
            _linkRenderer = linkRenderer;
        }
        public DownloadDictionaryView Render(DownloadJobModel source)
        {
            return new DownloadDictionaryView
            {
                Links = new List<LinkView>
                {
                    _linkRenderer.Render("CreateDictionaryDownload", RelTypes.Self, source.Type, new {id = source.Id})
                }
            };
        }
    }
}
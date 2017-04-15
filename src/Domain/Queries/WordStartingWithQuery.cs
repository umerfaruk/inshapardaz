﻿using Darker;

using Inshapardaz.Domain.Model;

namespace Inshapardaz.Domain.Queries
{
    public class WordStartingWithQuery : IQueryRequest<WordStartingWithQuery.Response>
    {
        public string Title { get; set; }

        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public class Response : IQueryResponse
        {
            public Page<Word> Page { get; set; } 
        }
    }
}
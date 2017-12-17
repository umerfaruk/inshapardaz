﻿using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Paramore.Darker;
using Inshapardaz.Domain.Database;
using Inshapardaz.Domain.Database.Entities;
using Inshapardaz.Domain.Helpers;
using Inshapardaz.Domain.Queries;
using Microsoft.EntityFrameworkCore;

namespace Inshapardaz.Domain.QueryHandlers
{
    public class GetWordRelationsByTitleQueryHandler : QueryHandlerAsync<GetWordRelationsByTitleQuery, Page<Word>>
    {
        private readonly IDatabaseContext _database;

        public GetWordRelationsByTitleQueryHandler(IDatabaseContext database)
        {
            _database = database;
        }

        public override async Task<Page<Word>> ExecuteAsync(GetWordRelationsByTitleQuery request, CancellationToken cancellationToken)
        {
            var relations = _database.Word
                            .Where(x => x.Title == request.Title)
                            .SelectMany(w => w.WordRelationRelatedWord)
                            .Where(r => r.RelationType == RelationType.Synonym)
                            .Select(x => x.SourceWord);

            var count = await relations.CountAsync(cancellationToken);
            var data = relations
                            .OrderBy(x => x.Title.Length)
                            .ThenBy(x => x.Title)
                            .Paginate(request.PageNumber, request.PageSize)
                            .ToList();

            return new Page<Word>
            {
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                TotalCount = count,
                Data = data
            };
        }
    }
}
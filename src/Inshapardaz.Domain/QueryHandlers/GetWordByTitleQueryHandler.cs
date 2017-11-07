﻿using Paramore.Darker;
using Inshapardaz.Domain.Queries;
using System.Threading;
using System.Threading.Tasks;
using Inshapardaz.Domain.Database;
using Inshapardaz.Domain.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Inshapardaz.Domain.QueryHandlers
{
    public class GetWordByTitleQueryHandler : QueryHandlerAsync<GetWordByTitleQuery, Word>
    {
        private readonly IDatabaseContext _database;

        public GetWordByTitleQueryHandler(IDatabaseContext database)
        {
            _database = database;
        }

        public override async Task<Word> ExecuteAsync(GetWordByTitleQuery query,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return await _database.Word.SingleOrDefaultAsync(
                x => x.DictionaryId == query.DictionaryId &&  
                     x.Title == query.Title,
                cancellationToken);
        }
    }
}
﻿using Inshapardaz.Domain.Queries;
using Paramore.Darker;
using System.Threading;
using System.Threading.Tasks;
using Inshapardaz.Domain.Database;
using Microsoft.EntityFrameworkCore;

namespace Inshapardaz.Domain.QueryHandlers
{
    public class DictionariesWordCountQueryHandler : QueryHandlerAsync<DictionariesWordCountQuery, int>
    {
        private readonly IDatabaseContext _database;

        public DictionariesWordCountQueryHandler(IDatabaseContext database)
        {
            _database = database;
        }

        public override async Task<int> ExecuteAsync(DictionariesWordCountQuery query,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var count =  await _database.Word.CountAsync(d => d.DictionaryId == query.DictionaryId, cancellationToken: cancellationToken);
            return count;
        }
    }
}
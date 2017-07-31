﻿using Darker;
using Inshapardaz.Domain.Queries;
using Inshapardaz.Domain.Model;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Inshapardaz.Domain.QueryHandlers
{
    public class WordDetailByIdQueryHandler : AsyncQueryHandler<WordDetailByIdQuery, WordDetail>
    {
        private readonly IDatabaseContext _database;

        public WordDetailByIdQueryHandler(IDatabaseContext database)
        {
            _database = database;
        }

        public override async Task<WordDetail> ExecuteAsync(WordDetailByIdQuery query,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return await _database.WordDetail.SingleOrDefaultAsync(w => w.Id == query.Id, cancellationToken);
        }
    }
}
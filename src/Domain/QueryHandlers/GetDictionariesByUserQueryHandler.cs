﻿using System.Collections.Generic;
using System.Linq;

using Inshapardaz.Domain.Queries;

using Darker;
using Inshapardaz.Domain.Model;

namespace Inshapardaz.Domain.QueryHandlers
{
    public class GetDictionariesByUserQueryHandler : QueryHandler<GetDictionariesByUserQuery, GetDictionariesByUserQuery.Response>
    {
        private readonly IDatabaseContext _database;

        public GetDictionariesByUserQueryHandler(IDatabaseContext database)
        {
            _database = database;
        }

        public override GetDictionariesByUserQuery.Response Execute(GetDictionariesByUserQuery request)
        {
            IQueryable<Dictionary> result;
            if (!string.IsNullOrWhiteSpace(request.UserId))
            {
                result = _database.Dictionaries.Where(d => d.IsPublic || (d.UserId == request.UserId));
            }
            else
            {
                result = _database.Dictionaries.Where(d => d.IsPublic);
            }

            return new GetDictionariesByUserQuery.Response(result.ToList());
        }
    }
}

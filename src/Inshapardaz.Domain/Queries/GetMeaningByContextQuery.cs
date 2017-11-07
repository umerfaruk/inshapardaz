using System.Collections.Generic;
using Paramore.Darker;
using Inshapardaz.Domain.Database.Entities;

namespace Inshapardaz.Domain.Queries
{
    public class GetWordMeaningsByContextQuery : IQuery<IEnumerable<Meaning>>
    {
        public GetWordMeaningsByContextQuery(long wordId, string context)
        {
            WordId = wordId;
            Context = context;
        }

        public long WordId { get; }

        public string Context { get; }
    }
}
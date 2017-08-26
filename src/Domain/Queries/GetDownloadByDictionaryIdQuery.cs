using Darker;
using Inshapardaz.Domain.Database.Entities;

namespace Inshapardaz.Domain.Queries
{
    public class GetDownloadByDictionaryIdQuery : IQuery<File>
    {
        public string UserId { get; set; }

        public int DictionaryId { get; set; }

        public string MimeType { get; set; }
    }
}
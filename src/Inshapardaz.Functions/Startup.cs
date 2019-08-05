using Inshapardaz.Domain.Repositories;
using Inshapardaz.Functions.Authentication;
using Inshapardaz.Functions.Configuration;
using Inshapardaz.Functions.Library.Categories;
using Inshapardaz.Functions.Library.Series;
using Inshapardaz.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;
using Inshapardaz.Functions.Library.Authors;
using Inshapardaz.Functions.Library.Books;
using Inshapardaz.Functions.Library.Books.Chapters;
using System.Linq;

[assembly: WebJobsStartup(typeof(Inshapardaz.Functions.Startup))]
namespace Inshapardaz.Functions
{
    public class Startup : IWebJobsStartup
    {
        public void Configure(IWebJobsBuilder builder)
        {
            builder.AddAccessTokenBinding();
            builder.Services.AddHttpClient()
                   .AddBrighterCommand()
                   .AddDatabase();
                
            if (!builder.Services.Any(x => x.ServiceType == typeof(IFileStorage)))
            {
                builder.Services.AddTransient<IFileStorage>(sp => new FileStorage(ConfigurationSettings.FileStorageConnectionString));
            }
        }
    }
}

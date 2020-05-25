﻿using Inshapardaz.Domain.Adapters;
using Inshapardaz.Domain.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace Inshapardaz.Domain.Helpers
{
    public class ImageHelper
    {
        public static async Task<string> TryConvertToPublicImage(int imageId, IFileRepository fileRepository, CancellationToken cancellationToken)
        {
            var image = await fileRepository.GetFileById(imageId, cancellationToken);
            if (image != null && image.IsPublic == true)
            {
                return image.FilePath.Replace(ConfigurationSettings.BlobRoot, ConfigurationSettings.CDNAddress);
            }

            return null;
        }
    }
}

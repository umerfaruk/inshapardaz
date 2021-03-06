﻿using Inshapardaz.Domain.Repositories;
using Paramore.Brighter;
using System.Threading;
using System.Threading.Tasks;

namespace Inshapardaz.Domain.Models
{
    public class DeleteFileRequest : RequestBase
    {
        public DeleteFileRequest(int imageId)
        {
            ImageId = imageId;
        }

        public int ImageId { get; private set; }
    }

    public class DeleteFileRequestHandler : RequestHandlerAsync<DeleteFileRequest>
    {
        private readonly IFileRepository _fileRepository;

        public DeleteFileRequestHandler(IFileRepository fileRepository)
        {
            _fileRepository = fileRepository;
        }

        public override async Task<DeleteFileRequest> HandleAsync(DeleteFileRequest command, CancellationToken cancellationToken = new CancellationToken())
        {
            await _fileRepository.DeleteFile(command.ImageId, cancellationToken);
            return await base.HandleAsync(command, cancellationToken);
        }
    }
}

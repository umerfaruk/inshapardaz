﻿using Inshapardaz.Domain.Models.Handlers.Library;
using Inshapardaz.Domain.Repositories;
using Inshapardaz.Domain.Repositories.Library;
using Paramore.Brighter;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Inshapardaz.Domain.Models.Library
{
    public class AddIssueContentRequest : LibraryBaseCommand
    {
        public AddIssueContentRequest(int libraryId, int periodicalId, int issueId, string language, string mimeType)
            : base(libraryId)
        {
            PeriodicalId = periodicalId;
            IssueId = issueId;
            Language = language;
            MimeType = mimeType;
        }

        public int PeriodicalId { get; }
        public int IssueId { get; }

        public string Language { get; }
        public string MimeType { get; }
        public FileModel Content { get; set; }

        public IssueContentModel Result { get; set; }
    }

    public class AddIssueContentRequestHandler : RequestHandlerAsync<AddIssueContentRequest>
    {
        private readonly IIssueRepository _issueRepository;
        private readonly IFileRepository _fileRepository;
        private readonly IFileStorage _fileStorage;

        public AddIssueContentRequestHandler(IIssueRepository issueRepository, IFileRepository fileRepository, IFileStorage fileStorage)
        {
            _issueRepository = issueRepository;
            _fileRepository = fileRepository;
            _fileStorage = fileStorage;
        }

        public override async Task<AddIssueContentRequest> HandleAsync(AddIssueContentRequest command, CancellationToken cancellationToken = new CancellationToken())
        {
            var issue = await _issueRepository.GetIssueById(command.LibraryId, command.PeriodicalId, command.IssueId, cancellationToken);

            if (issue != null)
            {
                var url = await StoreFile(issue.PeriodicalId, issue.Id, command.Content.FileName, command.Content.Contents, cancellationToken);
                command.Content.FilePath = url;
                command.Content.IsPublic = true;
                var file = await _fileRepository.AddFile(command.Content, cancellationToken);
                await _issueRepository.AddIssueContent(command.LibraryId, issue.PeriodicalId, issue.Id, file.Id, command.Language, command.MimeType, cancellationToken);

                command.Result = await _issueRepository.GetIssueContent(command.LibraryId, issue.PeriodicalId, issue.Id, command.Language, command.MimeType, cancellationToken);
            }

            return await base.HandleAsync(command, cancellationToken);
        }

        private async Task<string> StoreFile(int periodicalId, int issueId, string fileName, byte[] contents, CancellationToken cancellationToken)
        {
            var filePath = GetUniqueFileName(periodicalId, issueId, fileName);
            return await _fileStorage.StoreFile(filePath, contents, cancellationToken);
        }

        private static string GetUniqueFileName(int periodicalId, int issueId, string fileName)
        {
            var fileNameWithoutExtension = Path.GetExtension(fileName).Trim('.');
            return $"periodicals/{periodicalId}/issues/{issueId}/{Guid.NewGuid():N}.{fileNameWithoutExtension}";
        }
    }
}

﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Inshapardaz.Domain.Entities.Library;
using Inshapardaz.Domain.Exception;
using Inshapardaz.Domain.Repositories;
using Inshapardaz.Domain.Repositories.Library;
using Paramore.Brighter;

namespace Inshapardaz.Domain.Ports.Library
{
    public class GetChapterContentRequest : BookRequest
    {
        public GetChapterContentRequest(int bookId, int chapterId, string mimeType)
            : base(bookId)
        {
            ChapterId = chapterId;
            MimeType = mimeType;
        }

        public string MimeType { get; set; }

        public int ChapterId { get; }

        public ChapterContent Result { get; set; }
    }

    public class GetChapterContentRequestHandler : RequestHandlerAsync<GetChapterContentRequest>
    {
        private readonly IBookRepository _bookRepository;
        private readonly IChapterRepository _chapterRepository;

        public GetChapterContentRequestHandler(IBookRepository bookRepository, IChapterRepository chapterRepository)
        {
            _bookRepository = bookRepository;
            _chapterRepository = chapterRepository;
        }

        public override async Task<GetChapterContentRequest> HandleAsync(GetChapterContentRequest command, CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                var chapterContent = await _chapterRepository.GetChapterContent(command.BookId, command.ChapterId, command.MimeType, cancellationToken);
                if (chapterContent != null)
                {
                    await _bookRepository.AddRecentBook(command.UserId, command.BookId, cancellationToken);
                    command.Result = chapterContent;
                }
            }
            catch(System.Exception)
            {
            }

            return await base.HandleAsync(command, cancellationToken);
        }
    }
}


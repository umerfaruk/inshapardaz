﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Inshapardaz.Domain.Entities;
using Inshapardaz.Domain.Entities.Library;
using Inshapardaz.Domain.Repositories.Library;
using Paramore.Brighter;

namespace Inshapardaz.Domain.Ports.Library
{
    public class GetBooksRequest : RequestBase
    {
        public GetBooksRequest(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }

        public int PageNumber { get; private set; }

        public int PageSize { get; private set; }

        public Guid UserId { get; set; }

        public Page<Book> Result { get; set; }
    }

    public class GetBooksRequestHandler : RequestHandlerAsync<GetBooksRequest>
    {
        private readonly IBookRepository _bookRepository;

        public GetBooksRequestHandler(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public override async Task<GetBooksRequest> HandleAsync(GetBooksRequest command, CancellationToken cancellationToken = new CancellationToken())
        {
            var books = command.UserId == Guid.Empty
                ? await _bookRepository.GetPublicBooks(command.PageNumber, command.PageSize, cancellationToken)
                : await _bookRepository.GetBooks(command.PageNumber, command.PageSize, cancellationToken);
            command.Result = books;
            return await base.HandleAsync(command, cancellationToken);
        }
    }
}
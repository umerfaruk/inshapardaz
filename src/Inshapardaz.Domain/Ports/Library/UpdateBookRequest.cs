﻿using System.Threading;
using System.Threading.Tasks;
using Inshapardaz.Domain.Entities.Library;
using Inshapardaz.Domain.Repositories.Library;
using Paramore.Brighter;

namespace Inshapardaz.Domain.Ports.Library
{
    public class UpdateBookRequest : RequestBase
    {
        public UpdateBookRequest(Book book)
        {
            Book = book;
        }

        public Book Book { get; }

        public RequestResult Result { get; set; } = new RequestResult();

        public class RequestResult
        {
            public Book Book { get; set; }

            public bool HasAddedNew { get; set; }
        }
    }

    public class UpdateBookRequestHandler : RequestHandlerAsync<UpdateBookRequest>
    {
        private readonly IBookRepository _authorRepository;

        public UpdateBookRequestHandler(IBookRepository authorRepository)
        {
            _authorRepository = authorRepository;
        }

        public override async Task<UpdateBookRequest> HandleAsync(UpdateBookRequest command, CancellationToken cancellationToken = new CancellationToken())
        {
            var result = await _authorRepository.GetBookById(command.Book.Id, cancellationToken);

            if (result == null)
            {
                var author = command.Book;
                author.Id = default(int);
                command.Result.Book =  await  _authorRepository.AddBook(author, cancellationToken);
                command.Result.HasAddedNew = true;
            }
            else
            {
                await _authorRepository.UpdateBook(command.Book, cancellationToken);
                command.Result.Book = command.Book;
            }

            return await base.HandleAsync(command, cancellationToken);
        }
    }
}
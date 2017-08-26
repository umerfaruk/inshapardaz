﻿using System.Threading;
using System.Threading.Tasks;
using Inshapardaz.Domain.Commands;
using Inshapardaz.Domain.Database;
using paramore.brighter.commandprocessor;

namespace Inshapardaz.Domain.CommandHandlers
{
    public class AddWordCommandHandler : RequestHandlerAsync<AddWordCommand>
    {
        private readonly IDatabaseContext _database;

        public AddWordCommandHandler(IDatabaseContext database)
        {
            _database = database;
        }

        public override async Task<AddWordCommand> HandleAsync(AddWordCommand command, CancellationToken cancellationToken = default(CancellationToken))
        {
            await _database.Word.AddAsync(command.Word, cancellationToken);
            await _database.SaveChangesAsync(cancellationToken);

            return await  base.HandleAsync(command, cancellationToken);
        }
    }
}
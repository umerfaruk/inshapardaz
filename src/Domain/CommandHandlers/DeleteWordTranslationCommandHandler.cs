﻿using Darker;
using Inshapardaz.Domain.Commands;
using Inshapardaz.Domain.Exception;
using Inshapardaz.Domain.Model;
using Inshapardaz.Domain.Queries;
using paramore.brighter.commandprocessor;

namespace Inshapardaz.Domain.CommandHandlers
{
    public class DeleteWordTranslationCommandHandler : RequestHandler<DeleteWordTranslationCommand>
    {
        private readonly IDatabase _database;
        private readonly IQueryProcessor _queryProcessor;

        public DeleteWordTranslationCommandHandler(
            IDatabase database, IQueryProcessor queryProcessor)
        {
            _database = database;
            _queryProcessor = queryProcessor;
        }

        public override DeleteWordTranslationCommand Handle(DeleteWordTranslationCommand command)
        {
            var translation = _queryProcessor.Execute(new TranslationByIdQuery { Id = command.TranslationId });

            if (translation.Translation == null)
            {
                throw new RecordNotFoundException();
            }

            _database.Translations.Remove(translation.Translation);
            _database.SaveChanges();

            return base.Handle(command);
        }
    }
}
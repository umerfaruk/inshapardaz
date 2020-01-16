﻿using System.Threading;
using System.Threading.Tasks;
using Inshapardaz.Domain.Repositories.Dictionaries;
using Paramore.Brighter;

namespace Inshapardaz.Domain.Ports.Dictionaries
{
    public class UpdateDictionaryRequest : DictionaryRequest
    {
        public UpdateDictionaryRequest(int dictionaryId, Entities.Dictionaries.Dictionary dictionary)
            : base(dictionaryId)
        {
            Dictionary = dictionary;
        }

        public Entities.Dictionaries.Dictionary Dictionary { get; } 

        public UpdateDictionaryResult Result { get; set; } = new UpdateDictionaryResult();

        public class UpdateDictionaryResult
        {
            public bool HasAddedNew { get; set; }

            public Entities.Dictionaries.Dictionary Dictionary { get; set; }
        }
    }

    public class UpdateDictionaryRequestHandler : RequestHandlerAsync<UpdateDictionaryRequest>
    {
        private readonly IDictionaryRepository _dictionaryRepository;

        public UpdateDictionaryRequestHandler(IDictionaryRepository dictionaryRepository)
        {
            _dictionaryRepository = dictionaryRepository;
        }

        [DictionaryWriteRequestValidation(1, HandlerTiming.Before)]
        public override async Task<UpdateDictionaryRequest> HandleAsync(UpdateDictionaryRequest command, CancellationToken cancellationToken = new CancellationToken())
        {
            var result = await _dictionaryRepository.GetDictionaryById(command.DictionaryId, cancellationToken);

            if (result == null)
            {
                command.Dictionary.Id = default(int);
                var newCategory = await _dictionaryRepository.AddDictionary(command.Dictionary, cancellationToken);
                command.Result.HasAddedNew = true;
                command.Result.Dictionary = newCategory;
            }
            else
            {
                await _dictionaryRepository.UpdateDictionary(command.DictionaryId,  command.Dictionary, cancellationToken);
                command.Result.Dictionary = command.Dictionary;
            }
            
            return await base.HandleAsync(command, cancellationToken);
        }
    }
}
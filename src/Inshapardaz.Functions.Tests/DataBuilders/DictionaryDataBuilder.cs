using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Bogus;
using Inshapardaz.Ports.Database;
using Inshapardaz.Ports.Database.Entities;

namespace Inshapardaz.Functions.Tests.DataBuilders
{
    public class DictionaryDataBuilder
    {
        private readonly IDatabaseContext _context;
        private int _wordCount;
        private bool _private;

        private Guid _userId = Guid.Empty;

        public DictionaryDataBuilder(IDatabaseContext context)
        {
            _context = context;
        }

        public DictionaryDataBuilder WithWords(int wordCount)
        {
            _wordCount = wordCount;
            return this;
        }

        public DictionaryDataBuilder AsPrivate()
        {
            _private = true;
            return this;
        }

        public DictionaryDataBuilder ForUser(Guid userId)
        {
            _userId = userId;
            return this;
        }

        public Ports.Database.Entities.Dictionaries.Dictionary Build()
        {
            return Build(1).Single();
        }

        public IEnumerable<Ports.Database.Entities.Dictionaries.Dictionary> Build(int count)
        {
            var dictionaries = new Faker<Ports.Database.Entities.Dictionaries.Dictionary>()
                          .RuleFor(c => c.Id, 0)
                          .RuleFor(c => c.Name, f => f.Random.AlphaNumeric(10))
                          .RuleFor(c => c.IsPublic, !_private)
                          .RuleFor(c => c.UserId, _userId)
                          .Generate(count);

            foreach (var dictionary in dictionaries)
            {
                var words = new Faker<Ports.Database.Entities.Dictionaries.Word>()
                            .RuleFor(c => c.Id, 0)
                            .RuleFor(c => c.Title, f => f.Random.AlphaNumeric(10))
                            .RuleFor(c => c.TitleWithMovements, f => f.Random.AlphaNumeric(10))
                            .RuleFor(c => c.Dictionary, dictionary)
                            .Generate(_wordCount);

                dictionary.Word = words;
            }

            _context.Dictionary.AddRange(dictionaries);
            _context.SaveChanges();

            return dictionaries;
        }
    }
}
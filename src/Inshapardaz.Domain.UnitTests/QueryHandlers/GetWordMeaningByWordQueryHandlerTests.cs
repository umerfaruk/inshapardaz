﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FizzWare.NBuilder;
using Inshapardaz.Domain.Database.Entities;
using Inshapardaz.Domain.Queries;
using Inshapardaz.Domain.QueryHandlers;
using Shouldly;
using Xunit;

namespace Inshapardaz.Domain.UnitTests.QueryHandlers
{
    public class GetWordMeaningByWordQueryHandlerTests : DatabaseTest
    {
        private readonly GetWordMeaningsByWordQueryHandler _handler;
        private readonly Word _word;

        public GetWordMeaningByWordQueryHandlerTests()
        {
            _word = Builder<Word>
                .CreateNew()
                .Build();

            var meanings = Builder<Meaning>
                .CreateListOfSize(4)
                .Build();
            foreach (var meaning in meanings)
            {
                _word.Meaning.Add(meaning);
            }

            DbContext.Word.Add(_word);
            DbContext.SaveChanges();

            _handler = new GetWordMeaningsByWordQueryHandler(DbContext);
        }

        [Fact]
        public async Task WhenGettingContextMeaningFromAWord_ShouldReturnCorrectMeaning()
        {
            var meanings = await _handler.ExecuteAsync(new GetWordMeaningsByWordQuery(_word.Id));

            meanings.ShouldNotBeNull();
            meanings.ShouldNotBeEmpty();
            meanings.ShouldBe(_word.Meaning);
        }

        [Fact]
        public async Task WhenGettingContextMeaningFromIncorrectWord_ShouldReturnEmpty()
        {
            var meanings = await _handler.ExecuteAsync(new GetWordMeaningsByWordQuery(-1));

            meanings.ShouldNotBeNull();
            meanings.ShouldBeEmpty();
        }
    }
}
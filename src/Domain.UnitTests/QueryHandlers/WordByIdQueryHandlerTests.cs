﻿using Inshapardaz.Domain.Exception;
using Inshapardaz.Domain.Model;
using Inshapardaz.Domain.Queries;
using Inshapardaz.Domain.QueryHandlers;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Domain.UnitTests.QueryHandlers
{
    public class WordByIdQueryHandlerTests : DatabaseTestFixture
    {
        private WordByIdQueryHandler _handler;

        public WordByIdQueryHandlerTests()
        {
            _handler = new WordByIdQueryHandler(_database);

            _database.Dictionaries.Add(new Dictionary { Id = 1, UserId = "1", IsPublic = false });
            _database.Dictionaries.Add(new Dictionary { Id = 2, UserId = "1", IsPublic = true });
            _database.Words.Add(new Word { Id = 22, Title = "word1", DictionaryId = 1 });
            _database.Words.Add(new Word { Id = 23, Title = "word2", DictionaryId = 2 });
            _database.SaveChanges();
        }

        [Fact]
        public void WhenCallingForWordFromPublicDictionary_ShouldReturnWord()
        {
            var result = _handler.Execute(new WordByIdQuery { Id = 23, UserId = "1" });

            Assert.NotNull(result.Word);
            Assert.Equal(result.Word.Id, 23);
        }

        [Fact]
        public void WhenCallingForWordFromPrivateDictionary_ShouldReturnWord()
        {
            var result = _handler.Execute(new WordByIdQuery { Id = 22, UserId = "1" });

            Assert.NotNull(result.Word);
            Assert.Equal(result.Word.Id, 22);
        }

        [Fact]
        public void WhenCallingForWordFromPublicDictionaryAsAnonymousUser_ShouldReturnWord()
        {
            var result = _handler.Execute(new WordByIdQuery { Id = 23 });

            Assert.NotNull(result.Word);
            Assert.Equal(result.Word.Id, 23);
        }

        [Fact]
        public void WhenCallingForWordFromPrivateDictionaryAsAnonymousUser_ShouldNotReturnWord()
        {
            var result = _handler.Execute(new WordByIdQuery { Id = 22 });

            Assert.Null(result.Word);
        }

        [Fact]
        public void WhenCallingForInvalidWordFromPrivateDictionaryAsAnonymousUser_ShouldNotReturnWord()
        {
            var result = _handler.Execute(new WordByIdQuery { Id = 223 });

            Assert.Null(result.Word);
        }

    }
}

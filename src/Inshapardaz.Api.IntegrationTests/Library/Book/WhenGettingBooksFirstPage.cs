﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Inshapardaz.Api.View;
using Inshapardaz.Api.View.Library;
using Newtonsoft.Json;
using NUnit.Framework;
using Shouldly;

namespace Inshapardaz.Api.IntegrationTests.Library.Book
{
    [TestFixture]
    public class WhenGettingBooksFirstPage : IntegrationTestBase
    {
        private readonly List<Domain.Entities.Library.Book> _books = new List<Domain.Entities.Library.Book>();
        private Domain.Entities.Library.Author _author;
        private PageView<BookView> _view;

        [OneTimeSetUp]
        public async Task Setup()
        {
            _author = AuthorDataHelper.Create(new Domain.Entities.Library.Author {Name = "author1"});

            _books.Add(BookDataHelper.Create(new Domain.Entities.Library.Book {Title = "book 1", AuthorId = _author.Id}));
            _books.Add(BookDataHelper.Create(new Domain.Entities.Library.Book {Title = "book 2", AuthorId = _author.Id, IsPublic = false}));
            _books.Add(BookDataHelper.Create(new Domain.Entities.Library.Book {Title = "book 3", AuthorId = _author.Id}));
            _books.Add(BookDataHelper.Create(new Domain.Entities.Library.Book {Title = "book 4", AuthorId = _author.Id}));

            Response = await GetReaderClient(Guid.NewGuid()).GetAsync("/api/books?pageNumber=1&pageSize=2");
            _view = JsonConvert.DeserializeObject<PageView<BookView>>(await Response.Content.ReadAsStringAsync());
        }

        [OneTimeTearDown]
        public void Cleanup()
        {
            foreach (var book in _books)
            {
                BookDataHelper.Delete(book.Id);
            }

            AuthorDataHelper.Delete(_author.Id);
        }

        [Test]
        public void ShouldReturnOk()
        {
            Response.StatusCode.ShouldBe(HttpStatusCode.OK);
        }

        [Test]
        public void ShouldContainAllAuthor()
        {
            _view.Data.Count().ShouldBe(2);
        }

        [Test]
        public void ShouldReturnSelfLink()
        {
            _view.Links.ShouldContain(link => link.Rel == RelTypes.Self);
        }

        [Test]
        public void ShouldReturnCreateLink()
        {
            _view.Links.ShouldContain(link => link.Rel == RelTypes.Create);
        }

        [Test]
        public void ShouldNotReturnPreviousLink()
        {
            _view.Links.ShouldNotContain(link => link.Rel == RelTypes.Previous);
        }

        [Test]
        public void ShouldReturnNextLink()
        {
            _view.Links.ShouldContain(link => link.Rel == RelTypes.Next);
        }
    }
}
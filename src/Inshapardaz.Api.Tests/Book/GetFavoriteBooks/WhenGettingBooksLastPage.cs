﻿using Inshapardaz.Api.Tests.Asserts;
using Inshapardaz.Api.Views.Library;
using NUnit.Framework;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Inshapardaz.Api.Tests.Library.Book.GetFavoriteBooks
{
    [TestFixture]
    public class WhenGettingBooksLastPage
            : TestBase
    {
        private HttpResponseMessage _response;
        private PagingAssert<BookView> _assert;

        public WhenGettingBooksLastPage()
            : base(Domain.Adapters.Permission.Reader)
        {
        }

        [OneTimeSetUp]
        public async Task Setup()
        {
            BookBuilder.WithLibrary(LibraryId).IsPublic().AddToFavorites(UserId).Build(25);

            _response = await Client.GetAsync($"/library/{LibraryId}/books?pageNumber=3&pageSize=10&favorite=true");

            _assert = new PagingAssert<BookView>(_response, Library);
        }

        [OneTimeTearDown]
        public void Teardown()
        {
            Cleanup();
        }

        [Test]
        public void ShouldReturnOk()
        {
            _response.ShouldBeOk();
        }

        [Test]
        public void ShouldHaveSelfLink()
        {
            _assert.ShouldHaveSelfLink($"/library/{LibraryId}/books");
        }

        [Test]
        public void ShouldNotHaveNextLink()
        {
            _assert.ShouldNotHaveNextLink();
        }

        [Test]
        public void ShouldHavePreviousLink()
        {
            _assert.ShouldHavePreviousLink($"/library/{LibraryId}/books", 2);
        }

        [Test]
        public void ShouldReturnCorrectPage()
        {
            _assert.ShouldHavePage(3)
                   .ShouldHavePageSize(10)
                   .ShouldHaveTotalCount(BookBuilder.Books.Count())
                   .ShouldHaveItems(5);
        }

        [Test]
        public void ShouldReturnExpectedBooks()
        {
            var expectedItems = BookBuilder.Books.OrderBy(a => a.Title).Skip(2 * 10).Take(10);
            foreach (var item in expectedItems)
            {
                var actual = _assert.Data.FirstOrDefault(x => x.Id == item.Id);
                actual.ShouldMatch(item, DatabaseConnection)
                            .InLibrary(LibraryId)
                            .ShouldHaveCorrectLinks()
                            .ShouldNotHaveEditLinks()
                            .ShouldNotHaveImageUpdateLink()
                            .ShouldNotHaveCreateChaptersLink()
                            .ShouldNotHaveAddContentLink()
                            .ShouldHaveChaptersLink()
                            .ShouldHaveRemoveFavoriteLink()
                            .ShouldHavePublicImageLink();
            }
        }
    }
}
﻿using Inshapardaz.Api.Tests.Asserts;
using Inshapardaz.Api.Views.Library;
using NUnit.Framework;
using System.Net.Http;
using System.Threading.Tasks;

namespace Inshapardaz.Api.Tests.Library.Book.GetRecentReadBooks
{
    [TestFixture]
    public class WhenGettingRecentBooksPageThatDoesNotExist
        : TestBase
    {
        private HttpResponseMessage _response;
        private PagingAssert<BookView> _assert;

        public WhenGettingRecentBooksPageThatDoesNotExist()
            : base(Domain.Adapters.Permission.Reader)
        {
        }

        [OneTimeSetUp]
        public async Task Setup()
        {
            BookBuilder.WithLibrary(LibraryId)
                                       .IsPublic()
                                       .AddToRecentReads(UserId, 10)
                                       .Build(25);

            _response = await Client.GetAsync($"/library/{LibraryId}/books?pageNumber=3&pageSize=10&read=true");
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
            _assert.ShouldHaveSelfLink($"/library/{LibraryId}/books", "pageNumber", "3");
            _assert.ShouldHaveSelfLink($"/library/{LibraryId}/books", "pageSize", "10");
            _assert.ShouldHaveSelfLink($"/library/{LibraryId}/books", "read", bool.TrueString);
        }

        [Test]
        public void ShouldNotHaveNextLink()
        {
            _assert.ShouldNotHaveNextLink();
        }

        [Test]
        public void ShouldNotHavePreviousLink()
        {
            _assert.ShouldNotHavePreviousLink();
        }

        [Test]
        public void ShouldReturnCorrectPage()
        {
            _assert.ShouldHavePage(3)
                   .ShouldHavePageSize(10)
                   .ShouldHaveTotalCount(10)
                   .ShouldHaveItems(0);
        }

        [Test]
        public void ShouldReturnExpectedBooks()
        {
            _assert.ShouldHaveNoData();
        }
    }
}
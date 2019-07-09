﻿using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Inshapardaz.Functions.Tests.DataBuilders;
using Inshapardaz.Functions.Tests.Helpers;
using Inshapardaz.Functions.Views.Library;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;

namespace Inshapardaz.Functions.Tests.Library.Book.GetBookById
{
    [TestFixture]
    public class WhenGettingBookById : FunctionTest
    {
        OkObjectResult _response;
        BookView _view;
        private Ports.Database.Entities.Library.Book _expected;

        [OneTimeSetUp]
        public async Task Setup()
        {
            var request = TestHelpers.CreateGetRequest();

            var dataBuilder = Container.GetService<BooksDataBuilder>();
            var categories = dataBuilder.WithBooks(4, true).Build();
            _expected = categories.First();
            
            var handler = Container.GetService<Functions.Library.Books.GetBookById>();
            _response = (OkObjectResult) await handler.Run(request, NullLogger.Instance, _expected.Id, AuthenticationBuilder.Unauthorized, CancellationToken.None);

            _view = _response.Value as BookView;
        }

        [OneTimeTearDown]
        public void Teardown()
        {
            Cleanup();
        }

        [Test]
        public void ShouldHaveOkResult()
        {
            Assert.That(_response, Is.Not.Null);
            Assert.That(_response.StatusCode, Is.EqualTo(200));
        }

        [Test]
        public void ShouldHaveSelfLink()
        {
            _view.Links.AssertLink("self")
                 .ShouldBeGet()
                 .ShouldHaveSomeHref();
        }

        [Test]
        public void ShouldHaveAuthorLink()
        {
            _view.Links.AssertLink("author")
                 .ShouldBeGet()
                 .ShouldHaveSomeHref();
        }

        [Test]
        public void ShouldHaveChaptersLink()
        {
            _view.Links.AssertLink("chapters")
                 .ShouldBeGet()
                 .ShouldHaveSomeHref();
        }

        [Test]
        public void ShouldHaveFilesLink()
        {
            _view.Links.AssertLink("files")
                 .ShouldBeGet()
                 .ShouldHaveSomeHref();
        }

        [Test]
        public void ShouldHaveImageLink()
        {
            _view.Links.AssertLink("image")
                 .ShouldBeGet()
                 .ShouldHaveSomeHref();
        }

        [Test]
        public void ShouldHaveSeriesLink()
        {
            _view.Links.AssertLink("series")
                 .ShouldBeGet()
                 .ShouldHaveSomeHref();
        }

        [Test]
        public void ShouldReturnCorrectAuthorData()
        {
            Assert.That(_view, Is.Not.Null, "Should return book");
            Assert.That(_view.Id, Is.EqualTo(_expected.Id), "Book id does not match");
            Assert.That(_view.Title, Is.EqualTo(_expected.Title), "Book title does not match");
            Assert.That(_view.Description, Is.EqualTo(_expected.Description), "Book description count does not match");
            Assert.That(_view.Language, Is.EqualTo((int)_expected.Language), "Book language should match");
            Assert.That(_view.IsPublic, Is.EqualTo(_expected.IsPublic), "Book isPublic flag should match");
            Assert.That(_view.IsPublished, Is.EqualTo(_expected.IsPublished), "Book isPublished flag should match");
            Assert.That(_view.Copyrights, Is.EqualTo((int)_expected.Copyrights), "Book copyrights should match");
            Assert.That(_view.DateAdded, Is.EqualTo(_expected.DateAdded), "Book date added should match");
            Assert.That(_view.DateUpdated, Is.EqualTo(_expected.DateUpdated), "Book date updated should match");
            Assert.That(_view.Status, Is.EqualTo((int)_expected.Status), "Book status should match");
            Assert.That(_view.YearPublished, Is.EqualTo(_expected.YearPublished), "Book year published should match");
            Assert.That(_view.AuthorId, Is.EqualTo(_expected.Author.Id), "Book author id should match");
            Assert.That(_view.AuthorName, Is.EqualTo(_expected.Author.Name), "Book author name should match");
            Assert.That(_view.SeriesId, Is.EqualTo(_expected.Series.Id), "Book series id should match");
            Assert.That(_view.SeriesName, Is.EqualTo(_expected.Series.Name), "Book series name should match");
            Assert.That(_view.SeriesIndex, Is.EqualTo(_expected.SeriesIndex), "Book series index should match");
        }
    }
}
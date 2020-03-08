﻿using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Inshapardaz.Functions.Tests.DataBuilders;
using Inshapardaz.Functions.Tests.Dto;
using Inshapardaz.Functions.Tests.Helpers;
using Inshapardaz.Functions.Views.Library;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;

namespace Inshapardaz.Functions.Tests.Library.Chapter.GetChapterById
{
    [TestFixture]
    public class WhenGettingChapterByIdAsReader
        : LibraryTest<Functions.Library.Books.Chapters.GetChapterById>
    {
        private OkObjectResult _response;
        private ChapterView _view;
        private ChapterDto _expected;

        [OneTimeSetUp]
        public async Task Setup()
        {
            var dataBuilder = Container.GetService<ChapterDataBuilder>();
            _expected = dataBuilder.AsPublic().Build(4).First();

            _response = (OkObjectResult)await handler.Run(null, LibraryId, _expected.BookId, _expected.Id, AuthenticationBuilder.ReaderClaim, CancellationToken.None);

            _view = _response.Value as ChapterView;
        }

        [OneTimeTearDown]
        public void Teardown()
        {
            Cleanup();
        }

        [Test]
        public void ShouldReturnOk()
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
        public void ShouldHaveBookLink()
        {
            _view.Links.AssertLink("book")
                 .ShouldBeGet()
                 .ShouldHaveSomeHref();
        }

        [Test]
        public void ShouldNotHaveUpdateLink()
        {
            _view.Links.AssertLinkNotPresent("update");
        }

        [Test]
        public void ShouldNotHaveDeleteLink()
        {
            _view.Links.AssertLinkNotPresent("delete");
        }

        [Test]
        public void ShouldNotHaveAddChapterContentLink()
        {
            _view.Links.AssertLinkNotPresent("add-content");
        }

        [Test]
        public void ShouldNotHaveUpdateChapterContentLink()
        {
            _view.Links.AssertLinkNotPresent("update-content");
        }

        [Test]
        public void ShouldNotHaveDeleteChapterContentLink()
        {
            _view.Links.AssertLinkNotPresent("delete-contents");
        }

        [Test]
        public void ShouldReturnCorrectAuthorData()
        {
            Assert.That(_view, Is.Not.Null, "Should return chapter");
            Assert.That(_view.Id, Is.EqualTo(_expected.Id), "Chapter id does not match");
            Assert.That(_view.Title, Is.EqualTo(_expected.Title), "Chapter name does not match");
        }
    }
}

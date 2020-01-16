﻿using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Inshapardaz.Functions.Tests.DataBuilders;
using Inshapardaz.Functions.Tests.Helpers;
using Inshapardaz.Functions.Views.Library;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Inshapardaz.Functions.Tests.Library.Book.Files.GetBookFile
{
    [TestFixture]
    public class WhenGettingBookFileAsReader : FunctionTest
    {
        private OkObjectResult _response;

        private Ports.Database.Entities.Library.Book _book;
        private BookFilesView _view;
        private BooksDataBuilder _dataBuilder;

        [OneTimeSetUp]
        public async Task Setup()
        {
            _dataBuilder = Container.GetService<BooksDataBuilder>();

            _book = _dataBuilder.WithFiles(5).Build();
            var request = new RequestBuilder().Build();
            var handler = Container.GetService<Functions.Library.Books.Files.GetBookFiles>();
            _response = (OkObjectResult)await handler.Run(request, _book.Id, AuthenticationBuilder.ReaderClaim, CancellationToken.None);

            _view = (BookFilesView)_response.Value;
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
        public void ShouldReturnAllBookFiles()
        {
            foreach (var file in _book.Files)
            {
                var actual = _view.Items.SingleOrDefault(f => f.Id == file.File.Id);
                Assert.That(actual, Is.Not.Null, "File ot found in resonse");
                Assert.That(actual.FileName, Is.EqualTo(file.File.FileName), "File Name doesn't match");
                Assert.That(actual.MimeType, Is.EqualTo(file.File.MimeType), "MimeType doesn't match");
            }
        }
    }
}
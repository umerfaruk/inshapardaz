﻿using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Bogus;
using Inshapardaz.Functions.Tests.Asserts;
using Inshapardaz.Functions.Tests.DataBuilders;
using Inshapardaz.Functions.Tests.Dto;
using Inshapardaz.Functions.Tests.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Inshapardaz.Functions.Tests.Library.Book.Contents.UpdateBookFile
{
    [TestFixture(AuthenticationLevel.Administrator)]
    [TestFixture(AuthenticationLevel.Writer)]
    public class WhenUpdatingBookContentWithPermissions
        : LibraryTest<Functions.Library.Books.Content.UpdateBookContent>
    {
        private OkObjectResult _response;

        private BookDto _book;
        private BookContentDto _file;
        private byte[] _expected;
        private BooksDataBuilder _dataBuilder;
        private BookContentAssert _assert;
        private ClaimsPrincipal _claim;

        public WhenUpdatingBookContentWithPermissions(AuthenticationLevel authenticationLevel)
        {
            _claim = AuthenticationBuilder.CreateClaim(authenticationLevel);
        }

        [OneTimeSetUp]
        public async Task Setup()
        {
            _dataBuilder = Container.GetService<BooksDataBuilder>();

            _book = _dataBuilder.WithLibrary(LibraryId).WithContents(2).Build();
            _file = _dataBuilder.Contents.PickRandom();

            _expected = new Faker().Image.Random.Bytes(50);

            var request = new RequestBuilder()
                .WithBytes(_expected)
                .WithContentType(_file.MimeType)
                .WithLanguage(_file.Language)
                .BuildRequestMessage();

            _response = (OkObjectResult)await handler.Run(request, LibraryId, _book.Id, _claim, CancellationToken.None);

            _assert = new BookContentAssert(_response, LibraryId);
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
        public void ShouldHaveUpdatedFileContents()
        {
            _assert.ShouldHaveBookContent(_expected, DatabaseConnection, FileStorage);
        }
    }
}
﻿using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Inshapardaz.Functions.Tests.Asserts;
using Inshapardaz.Functions.Tests.DataBuilders;
using Inshapardaz.Functions.Tests.Helpers;
using Inshapardaz.Functions.Views;
using Inshapardaz.Functions.Views.Library;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Inshapardaz.Functions.Tests.Library.Author.GetAuthors
{
    [TestFixture]
    public class WhenGettingAuthorsFirstPage : LibraryTest<Functions.Library.Authors.GetAuthors>
    {
        private AuthorsDataBuilder _builder;
        private OkObjectResult _response;
        private PagingAssert<AuthorView> _assert;

        [OneTimeSetUp]
        public async Task Setup()
        {
            var request = new RequestBuilder()
                .WithQueryParameter("pageNumber", 1)
                .WithQueryParameter("pageSize", 10)
                .Build();

            _builder = Container.GetService<AuthorsDataBuilder>();
            _builder.WithLibrary(LibraryId).WithBooks(3).Build(20);

            _response = (OkObjectResult)await handler.Run(request, LibraryId, AuthenticationBuilder.Unauthorized, CancellationToken.None);

            _assert = new PagingAssert<AuthorView>(_response);
        }

        [OneTimeTearDown]
        public void Teardown()
        {
            _builder.CleanUp();
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
            _assert.ShouldHaveSelfLink($"/api/library/{LibraryId}/authors");
        }

        [Test]
        public void ShouldHaveNextLink()
        {
            _assert.ShouldHaveNextLink($"/api/library/{LibraryId}/authors", 2, 10);
        }

        [Test]
        public void ShouldNotHavePreviousLink()
        {
            _assert.ShouldNotHavePreviousLink();
        }

        [Test]
        public void ShouldHaveCorrectAuthorData()
        {
            var expectedItems = _builder.Authors.OrderBy(a => a.Name).Take(10);
            foreach (var item in expectedItems)
            {
                var actual = _assert.Data.FirstOrDefault(x => x.Id == item.Id);

                actual.Should().NotBeNull();
                actual.Name.Should().Be(item.Name);
                actual.BookCount.Should().Be(3);

                actual.Links.AssertLinkNotPresent("update");
                actual.Links.AssertLinkNotPresent("delete");
            }
        }
    }
}

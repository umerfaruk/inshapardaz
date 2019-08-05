﻿using System.Net;
using System.Threading.Tasks;
using Inshapardaz.Functions.Tests.DataBuilders;
using Inshapardaz.Functions.Tests.Helpers;
using Inshapardaz.Functions.Views.Library;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Inshapardaz.Functions.Tests.Library.Author.UploadAuthorImage
{
    [TestFixture, Ignore("Add content upload to test message")]
    public class WhenUploadingAuthorAsWriter : FunctionTest
    {
        CreatedResult _response;
        private AuthorsDataBuilder _builder;

        [OneTimeSetUp]
        public async Task Setup()
        {
            _builder = Container.GetService<AuthorsDataBuilder>();
            
            var handler = Container.GetService<Functions.Library.Authors.UpdateAuthorImage>();
            var request = new RequestBuilder().Build();
            //_response = (CreatedResult) await handler.Run(request, NullLogger.Instance, 1, AuthenticationBuilder.WriterClaim, CancellationToken.None);
        }

        [OneTimeTearDown]
        public void Teardown()
        {
            Cleanup();
        }

        [Test]
        public void ShouldHaveCreatedResult()
        {
            Assert.That(_response, Is.Not.Null);
            Assert.That(_response.StatusCode, Is.EqualTo((int)HttpStatusCode.Created));
        }

        [Test]
        public void ShouldHaveLocationHeader()
        {
            Assert.That(_response.Location, Is.Not.Empty);
        }

        [Test]
        public void ShouldHaveCreatedTheAuthor()
        {
            var actual = _response.Value as AuthorView;
            Assert.That(actual, Is.Not.Null);

            var cat = _builder.GetById(actual.Id);
            Assert.That(cat, Is.Not.Null, "Author should be created.");
        }
    }
}
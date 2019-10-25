﻿using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Inshapardaz.Domain.Repositories;
using Inshapardaz.Functions.Tests.DataBuilders;
using Inshapardaz.Functions.Tests.Fakes;
using Inshapardaz.Functions.Tests.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Inshapardaz.Functions.Tests.Library.Author.UploadAuthorImage
{
    [TestFixture]
    public class WhenUploadingAuthorImageAsReader : FunctionTest
    {
        private ForbidResult _response;
        private AuthorsDataBuilder _builder;
        private FakeFileStorage _fileStorage;
        private int _authorId;
        private byte[] _oldImage;
        [OneTimeSetUp]
        public async Task Setup()
        {
            _builder = Container.GetService<AuthorsDataBuilder>();
            _fileStorage = Container.GetService<IFileStorage>() as FakeFileStorage;
            
            var author = _builder.Build();
            _authorId = author.Id;
            var imageUrl = _builder.GetAuthorImageUrl(_authorId);
            _oldImage = await _fileStorage.GetFile(imageUrl, CancellationToken.None);
            var handler = Container.GetService<Functions.Library.Authors.UpdateAuthorImage>();
            var request = new RequestBuilder().WithImage().BuildRequestMessage();
            _response = (ForbidResult) await handler.Run(request, _authorId, AuthenticationBuilder.ReaderClaim, CancellationToken.None);
        }

        [OneTimeTearDown]
        public void Teardown()
        {
            Cleanup();
        }

        [Test]
        public void ShouldHaveForbidResult()
        {
            Assert.That(_response, Is.Not.Null);
        }


        [Test]
        public async Task ShouldHaveUpdatedAuthorImage()
        {
            var imageUrl = _builder.GetAuthorImageUrl(_authorId);
            Assert.That(imageUrl, Is.Not.Null, "Author should have an image url.");
            var image = await _fileStorage.GetFile(imageUrl, CancellationToken.None);
            Assert.That(image, Is.Not.Null, "Author should have an image.");
            Assert.That(image, Is.EqualTo(_oldImage), "Author image should not be updated.");
        }
    }
}
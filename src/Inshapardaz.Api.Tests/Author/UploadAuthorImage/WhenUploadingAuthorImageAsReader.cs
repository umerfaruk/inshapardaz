﻿using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Inshapardaz.Api.Tests.Asserts;
using Inshapardaz.Api.Tests.DataHelpers;
using Inshapardaz.Api.Tests.Helpers;
using Inshapardaz.Domain.Adapters;
using NUnit.Framework;

namespace Inshapardaz.Api.Tests.Author.UploadAuthorImage
{
    [TestFixture]
    public class WhenUploadingAuthorImageAsReader : TestBase
    {
        private HttpResponseMessage _response;
        private int _authorId;
        private byte[] _oldImage;

        public WhenUploadingAuthorImageAsReader()
            : base(Permission.Reader)
        {
        }

        [OneTimeSetUp]
        public async Task Setup()
        {
            var author = AuthorBuilder.WithLibrary(LibraryId).Build();
            _authorId = author.Id;
            var imageUrl = DatabaseConnection.GetAuthorImageUrl(_authorId);
            _oldImage = await FileStore.GetFile(imageUrl, CancellationToken.None);
            var newimage = Random.Bytes;
            var client = CreateClient();
            _response = await client.PutFile($"/library/{LibraryId}/authors/{_authorId}/image", newimage);
        }

        [OneTimeTearDown]
        public void Teardown()
        {
            Cleanup();
        }

        [Test]
        public void ShouldHaveForbidResult()
        {
            _response.ShouldBeForbidden();
        }

        [Test]
        public void ShouldNotHaveUpdatedAuthorImage()
        {
            AuthorAssert.ShouldNotHaveUpdatedAuthorImage(_authorId, _oldImage, DatabaseConnection, FileStore);
        }
    }
}

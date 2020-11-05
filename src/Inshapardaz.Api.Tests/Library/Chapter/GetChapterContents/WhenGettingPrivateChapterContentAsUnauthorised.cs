﻿using Inshapardaz.Api.Tests.Asserts;
using Inshapardaz.Api.Tests.Dto;
using Inshapardaz.Api.Tests.Helpers;
using NUnit.Framework;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Inshapardaz.Api.Tests.Library.Chapter.Contents.GetChapterContents
{
    [TestFixture]
    public class WhenGettingPrivateChapterContentAsUnauthorised
        : TestBase
    {
        private HttpResponseMessage _response;
        private ChapterDto _chapter;

        [OneTimeSetUp]
        public async Task Setup()
        {
            _chapter = ChapterBuilder.WithLibrary(LibraryId).Private().WithContents().Build();
            var content = ChapterBuilder.Contents.Single(x => x.ChapterId == _chapter.Id);
            var file = ChapterBuilder.Files.Single(x => x.Id == content.FileId);
            var contents = FileStore.GetFile(file.FilePath, CancellationToken.None).Result;

            _response = await Client.GetAsync($"/library/{LibraryId}/books/{_chapter.BookId}/chapters/{_chapter.Id}/contents", content.Language, file.MimeType);
        }

        [OneTimeTearDown]
        public void Teardown()
        {
            Cleanup();
        }

        [Test]
        public void ShouldReturnUnathorised()
        {
            _response.ShouldBeUnauthorized();
        }
    }
}
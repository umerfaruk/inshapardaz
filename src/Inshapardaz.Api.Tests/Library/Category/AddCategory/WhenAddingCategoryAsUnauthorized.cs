﻿using System.Net.Http;
using System.Threading.Tasks;
using Inshapardaz.Api.Tests.Asserts;
using Inshapardaz.Api.Tests.Helpers;
using Inshapardaz.Api.Views.Library;
using NUnit.Framework;

namespace Inshapardaz.Api.Tests.Library.Categories.AddCategory
{
    [TestFixture]
    public class WhenAddingCategoryAsUnauthorized : TestBase
    {
        private HttpResponseMessage _response;

        [OneTimeSetUp]
        public async Task Setup()
        {
            var category = new CategoryView { Name = Random.Name };

            _response = await Client.PostObject($"/libraries/{LibraryId}/categories", category);
        }

        [OneTimeTearDown]
        public void Teardown()
        {
            Cleanup();
        }

        [Test]
        public void ShouldHaveUnauthorizedResult()
        {
            _response.ShouldBeUnauthorized();
        }
    }
}
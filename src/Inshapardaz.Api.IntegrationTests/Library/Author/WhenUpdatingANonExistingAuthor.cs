﻿using System;
using System.Net;
using System.Threading.Tasks;
using Inshapardaz.Api.IntegrationTests.Helpers;
using Inshapardaz.Api.View;
using Inshapardaz.Api.View.Library;
using Newtonsoft.Json;
using NUnit.Framework;
using Shouldly;

namespace Inshapardaz.Api.IntegrationTests.Library.Author
{
    [TestFixture]
    public class WhenUpdatingANonExistingAuthor : IntegrationTestBase
    {
        private AuthorView _view;
        private AuthorView _auhtor;
        private readonly Guid _userId = Guid.NewGuid();

        [OneTimeSetUp]
        public async Task Setup()
        {
            _auhtor = new AuthorView
            {
                Id = -3434,
                Name = "AuthorName"
            };

            Response = await GetAdminClient(_userId).PutJson($"/api/authors/{_auhtor.Id}", _auhtor);
            _view = JsonConvert.DeserializeObject<AuthorView>(await Response.Content.ReadAsStringAsync());
        }

        [OneTimeTearDown]
        public void Cleanup()
        {
            AuthorDataHelper.Delete(_view.Id);
        }

        [Test]
        public void ShouldReturnCreated()
        {
            Response.StatusCode.ShouldBe(HttpStatusCode.Created);
        }

        [Test]
        public void ShouldReturnLocationHeader()
        {
            Response.Headers.Location.ShouldNotBeNull();
        }

        [Test]
        public void ShouldReturnSelfLink()
        {
            _view.Links.ShouldContain(l => l.Rel == RelTypes.Self & l.Href != null);
        }

        [Test]
        public void ShouldReturnUpdateLink()
        {
            _view.Links.ShouldContain(l => l.Rel == RelTypes.Update & l.Href != null);
        }

        [Test]
        public void ShouldReturnDeleteLink()
        {
            _view.Links.ShouldContain(l => l.Rel == RelTypes.Delete & l.Href != null);
        }
        
        [Test]
        public void ShouldReturnCorrectMeaningData()
        {
            _view.Name.ShouldBe(_auhtor.Name);
        }
    }
}
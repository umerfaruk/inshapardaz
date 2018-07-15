﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Inshapardaz.Api.View;
using Inshapardaz.Api.View.Dictionary;
using Inshapardaz.Domain.Entities;
using Inshapardaz.Domain.Entities.Dictionary;
using Newtonsoft.Json;
using NUnit.Framework;
using Shouldly;

namespace Inshapardaz.Api.IntegrationTests.Dictionary.Dictionary
{
    [TestFixture]
    public class WhenGettingAPrivateDictionaryAsAnonymousUser : IntegrationTestBase
    {
        private DictionaryView _view;
        private Domain.Entities.Dictionary.Dictionary _dictionary;

        [OneTimeSetUp]
        public async Task Setup()
        {
            _dictionary = new Domain.Entities.Dictionary.Dictionary
            {
                IsPublic = false,
                Name = "Test1",
                UserId = Guid.NewGuid(),
                Downloads = new List<DictionaryDownload>
                {
                    new DictionaryDownload { File = "223323", MimeType = MimeTypes.SqlLite}
                }
            };
            _dictionary = DictionaryDataHelper.CreateDictionary(_dictionary);

            Response = await GetClient().GetAsync($"/api/dictionaries/{_dictionary.Id}");
            _view = JsonConvert.DeserializeObject<DictionaryView>(await Response.Content.ReadAsStringAsync());
        }

        [OneTimeTearDown]
        public void Cleanup()
        {
            DictionaryDataHelper.DeleteDictionary(-1);
        }

        [Test]
        public void ShouldReturnUnAuthorised()
        {
            Response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
        }

        [Test]
        public void ShouldNotHaveDataInResponseBody()
        {
            Assert.That(_view, Is.Null);
        }
    }
}
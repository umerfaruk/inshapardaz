﻿using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Inshapardaz.Functions.Tests.DataBuilders;
using Inshapardaz.Ports.Database.Entities.Library;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;

namespace Inshapardaz.Functions.Tests.Library.Categories.DeleteCategory
{
    [TestFixture]
    public class WhenDeletingCategoryAsReader : FunctionTest
    {
        ForbidResult _response;

        private IEnumerable<Category> _categories;
        private Category _selectedCategory;
        private CategoriesDataBuilder _categoriesBuilder;

        [OneTimeSetUp]
        public async Task Setup()
        {
            var request = TestHelpers.CreateGetRequest();
            AuthenticateAsReader();
            _categoriesBuilder = Container.GetService<CategoriesDataBuilder>();
            _categories = _categoriesBuilder.WithCategories(4).Build();
            _selectedCategory = _categories.First();
            
            var handler = Container.GetService<Functions.Library.Categories.DeleteCategory>();
            _response = (ForbidResult) await handler.Run(request, NullLogger.Instance, _selectedCategory.Id , CancellationToken.None);
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
        }
    }
}
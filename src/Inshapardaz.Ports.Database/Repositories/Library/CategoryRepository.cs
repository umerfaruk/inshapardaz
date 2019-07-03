﻿using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Inshapardaz.Domain.Entities.Library;
using Inshapardaz.Domain.Exception;
using Inshapardaz.Domain.Repositories.Library;
using Microsoft.EntityFrameworkCore;

namespace Inshapardaz.Ports.Database.Repositories.Library
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly IDatabaseContext _databaseContext;

        public CategoryRepository(IDatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<Category> AddCategory(Category category, CancellationToken cancellationToken)
        {
            var item = category.Map();
            _databaseContext.Category.Add(item);

            await _databaseContext.SaveChangesAsync(cancellationToken);

            return item.Map();
        }

        public async Task UpdateCategory(Category category, CancellationToken cancellationToken)
        {
            var existingEntity = await _databaseContext.Category
                                                       .SingleOrDefaultAsync(g => g.Id == category.Id,
                                                                             cancellationToken);

            if (existingEntity == null)
            {
                throw new NotFoundException();
            }

            existingEntity.Name = category.Name;

            await _databaseContext.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteCategory(int categoryId, CancellationToken cancellationToken)
        {
            var category = await _databaseContext.Category.SingleOrDefaultAsync(g => g.Id == categoryId, cancellationToken);

            if (category == null)
            {
                throw new NotFoundException();
            }

            _databaseContext.Category.Remove(category);
            await _databaseContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<IEnumerable<Category>> GetCategory(CancellationToken cancellationToken)
        {
            return await _databaseContext.Category
                                         .Select(t => t.Map())
                                         .ToListAsync(cancellationToken);
        }

        public async Task<Category> GetCategoryById(int categoryId, CancellationToken cancellationToken)
        {
            var category = await _databaseContext.Category
                                                    .SingleOrDefaultAsync(t => t.Id == categoryId,
                                                                          cancellationToken);
            return category.Map();
        }
    }
}
﻿using Inshapardaz.Domain.Models;
using Inshapardaz.Domain.Models.Library;
using Inshapardaz.Functions.Views.Library;
using System.Linq;

namespace Inshapardaz.Functions.Mappings
{
    public static class BookMapper
    {
        public static BookView Map(this BookModel source)
            => new BookView
            {
                Id = source.Id,
                Title = source.Title,
                Description = source.Description,
                AuthorId = source.AuthorId,
                AuthorName = source.AuthorName,
                IsPublic = source.IsPublic,
                Language = (int)source.Language,
                DateAdded = source.DateAdded,
                DateUpdated = source.DateUpdated,
                SeriesId = source.SeriesId,
                SeriesName = source.SeriesName,
                SeriesIndex = source.SeriesIndex,
                Copyrights = (int)source.Copyrights,
                Status = (int)source.Status,
                YearPublished = source.YearPublished,
                IsPublished = source.IsPublished,
                Categories = source.Categories?.Select(c => c.Map())
            };

        public static BookModel Map(this BookView source)
            => new BookModel
            {
                Id = source.Id,
                Title = source.Title,
                Description = source.Description,
                AuthorId = source.AuthorId,
                IsPublic = source.IsPublic,
                Language = (Languages)source.Language,
                DateAdded = source.DateAdded,
                DateUpdated = source.DateUpdated,
                SeriesId = source.SeriesId,
                SeriesIndex = source.SeriesIndex,
                Copyrights = (CopyrightStatuses)source.Copyrights,
                Status = (BookStatuses)source.Status,
                YearPublished = source.YearPublished,
                IsPublished = source.IsPublished,
                Categories = source.Categories?.Select(c => c.Map()).ToList()
            };
    }
}

﻿using FluentAssertions;
using Inshapardaz.Api.Tests.DataHelpers;
using Inshapardaz.Api.Tests.Dto;
using Inshapardaz.Api.Tests.Helpers;
using Inshapardaz.Api.Views;
using Inshapardaz.Api.Views.Library;
using Inshapardaz.Domain.Repositories;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Threading;

namespace Inshapardaz.Api.Tests.Asserts
{
    public class BookAssert
    {
        private BookView _book;
        private int _libraryId;

        public HttpResponseMessage _response;

        public BookAssert(HttpResponseMessage response)
        {
            _response = response;
            _book = response.GetContent<BookView>().Result;
        }

        public BookAssert(BookView view)
        {
            _book = view;
        }

        internal static BookAssert WithResponse(HttpResponseMessage response)
        {
            return new BookAssert(response);
        }

        public BookAssert InLibrary(int libraryId)
        {
            _libraryId = libraryId;
            return this;
        }

        public BookAssert ShouldHaveSelfLink()
        {
            _book.SelfLink()
                  .ShouldBeGet()
                  .EndingWith($"library/{_libraryId}/books/{_book.Id}");

            return this;
        }

        public BookAssert ShouldHaveAuthorLink()
        {
            _book.Link("author")
                  .ShouldBeGet()
                  .EndingWith($"library/{_libraryId}/authors/{_book.AuthorId}");

            return this;
        }

        public BookAssert ShouldHaveChaptersLink()
        {
            _book.Link("chapters")
                  .ShouldBeGet()
                  .EndingWith($"library/{_libraryId}/books/{_book.Id}/chapters");

            return this;
        }

        public BookAssert ShouldHaveAddFavoriteLink()
        {
            _book.Link(RelTypes.CreateFavorite)
                  .ShouldBePost()
                  .EndingWith($"library/{_libraryId}/favorites/books/{_book.Id}");

            return this;
        }

        public BookAssert ShouldNotHaveAddFavoriteLink()
        {
            _book.Link(RelTypes.CreateFavorite).Should().BeNull();
            return this;
        }

        public BookAssert ShouldHaveRemoveFavoriteLink()
        {
            _book.Link(RelTypes.RemoveFavorite)
                  .ShouldBeDelete()
                  .EndingWith($"library/{_libraryId}/favorites/books/{_book.Id}");

            return this;
        }

        public BookAssert ShouldNotHaveRemoveFavoriteLink()
        {
            _book.Link(RelTypes.RemoveFavorite).Should().BeNull();
            return this;
        }

        public BookAssert ShouldHaveContents(IDbConnection db, bool haveEditableLinks = false)
        {
            var bookContents = db.GetBookContents(_book.Id);
            _book.Contents.Should().NotBeEmpty();
            _book.Contents.Should().HaveSameCount(bookContents);

            foreach (var content in bookContents)
            {
                var bookContent = _book.Contents.SingleOrDefault(c => c.Language == content.Language && c.MimeType == content.MimeType);
                bookContent.Should().NotBeNull();
                bookContent.Link("self")
                    .ShouldBeGet()
                    .ShouldHaveAcceptLanguage(content.Language)
                    .ShouldHaveAccept(content.MimeType)
                    .EndingWith($"library/{_libraryId}/books/{_book.Id}/content");

                bookContent.Link("book")
                    .ShouldBeGet()
                    .EndingWith($"library/{_libraryId}/books/{_book.Id}");

                if (haveEditableLinks)
                {
                    bookContent.Link("update")
                                        .ShouldBePut()
                                        .ShouldHaveAcceptLanguage(content.Language)
                                        .ShouldHaveAccept(content.MimeType)
                                        .EndingWith($"library/{_libraryId}/books/{_book.Id}/content");
                    bookContent.Link("delete")
                                        .ShouldBeDelete()
                                        .ShouldHaveAcceptLanguage(content.Language)
                                        .ShouldHaveAccept(content.MimeType)
                                        .EndingWith($"library/{_libraryId}/books/{_book.Id}/content");
                }
                else
                {
                    bookContent.Link("update").Should().BeNull();
                    bookContent.Link("delete").Should().BeNull();
                }
            }

            return this;
        }

        internal BookAssert ShouldHaveEditLinks()
        {
            return ShouldHaveUpdateLink()
                   .ShouldHaveDeleteLink();
        }

        internal BookAssert ShouldNotHaveEditLinks()
        {
            return ShouldNotHaveUpdateLink()
                   .ShouldNotHaveDeleteLink();
        }

        internal BookAssert ShouldHaveCorrectLinks()
        {
            ShouldHaveSelfLink();
            ShouldHaveAuthorLink();
            return this;
        }

        internal BookAssert ShouldHaveCorrectImageLocationHeader(int bookId)
        {
            _response.Headers.Location.AbsoluteUri.Should().NotBeEmpty();
            return this;
        }

        internal static void ShouldHavePublicImage(int bookId, IDbConnection dbConnection)
        {
            var image = dbConnection.GetBookImage(bookId);
            image.Should().NotBeNull();
            image.IsPublic.Should().BeTrue();
        }

        public BookAssert ShouldHaveSeriesLink()
        {
            _book.Link("series")
                  .ShouldBeGet()
                  .EndingWith($"library/{_libraryId}/series/{_book.SeriesId}");

            return this;
        }

        public BookAssert ShouldNotHaveSeriesLink()
        {
            _book.Link("series").Should().BeNull();
            return this;
        }

        public BookAssert ShouldHaveUpdateLink()
        {
            _book.UpdateLink()
                  .ShouldBePut()
                  .EndingWith($"library/{_libraryId}/books/{_book.Id}");

            return this;
        }

        public BookAssert ShouldNotHaveUpdateLink()
        {
            _book.UpdateLink().Should().BeNull();
            return this;
        }

        public BookAssert ShouldNotHaveDeleteLink()
        {
            _book.DeleteLink().Should().BeNull();
            return this;
        }

        public BookAssert ShouldNotHaveImageUpdateLink()
        {
            _book.Link("image-upload").Should().BeNull();
            return this;
        }

        public BookAssert ShouldHaveImageUpdateLink()
        {
            _book.Link("image-upload")
                   .ShouldBePut()
                   .EndingWith($"library/{_libraryId}/books/{_book.Id}/image");
            return this;
        }

        public BookAssert ShouldHaveDeleteLink()
        {
            _book.DeleteLink()
                  .ShouldBeDelete()
                  .EndingWith($"library/{_libraryId}/books/{_book.Id}");
            return this;
        }

        public BookAssert ShouldHaveCreateChaptersLink()
        {
            _book.Link("create-chapter")
                  .ShouldBePost()
                  .EndingWith($"library/{_libraryId}/books/{_book.Id}/chapters");
            return this;
        }

        public BookAssert ShouldNotHaveCreateChaptersLink()
        {
            _book.Link("create-chapter").Should().BeNull();
            return this;
        }

        public BookAssert ShouldHaveAddContentLink()
        {
            _book.Link("add-file")
                  .ShouldBePost()
                  .EndingWith($"library/{_libraryId}/books/{_book.Id}/content");
            return this;
        }

        public BookAssert ShouldNotHaveAddContentLink()
        {
            _book.Link("add-file").Should().BeNull();
            return this;
        }

        internal BookAssert ShouldHavePublicImageLink()
        {
            _book.Link("image")
                .ShouldBeGet();
            //.Href.Should().StartWith(Settings.CDNAddress);

            return this;
        }

        internal BookAssert ShouldHaveCorrectLocationHeader()
        {
            _response.Headers.Location.Should().NotBeNull();
            _response.Headers.Location.AbsoluteUri.Should().EndWith($"library/{_libraryId}/books/{_book.Id}");
            return this;
        }

        public BookAssert ShouldHaveSavedBook(IDbConnection dbConnection)
        {
            var dbbook = dbConnection.GetBookById(_book.Id);
            return ShouldBeSameAs(dbbook, dbConnection);
        }

        internal static void ShouldHaveDeletedBookFromFavorites(int bookId, IDbConnection dbConnection)
        {
            dbConnection.DoesBookExistsInFavorites(bookId).Should().BeFalse();
        }

        internal static void ShouldHaveDeletedBookFromRecentReads(int bookId, IDbConnection dbConnection)
        {
            dbConnection.DoesBookExistsInRecent(bookId).Should().BeFalse();
        }

        internal BookAssert ShouldBeSameAs(BookDto expected, IDbConnection db)
        {
            _book.Should().NotBeNull();
            _book.Title.Should().Be(expected.Title);
            _book.Description.Should().Be(expected.Description);
            _book.Language.Should().Be((int)expected.Language);

            _book.IsPublic.Should().Be(expected.IsPublic);
            _book.IsPublished.Should().Be(expected.IsPublished);
            _book.Copyrights.Should().Be((int)expected.Copyrights);
            _book.DateAdded.Should().BeCloseTo(expected.DateAdded);
            _book.DateUpdated.Should().BeCloseTo(expected.DateUpdated);
            _book.Status.Should().Be((int)expected.Status);
            _book.YearPublished.Should().Be(expected.YearPublished);
            _book.AuthorId.Should().Be(expected.AuthorId);
            _book.AuthorName.Should().Be(db.GetAuthorById(expected.AuthorId).Name);
            _book.SeriesId.Should().Be(expected.SeriesId);
            if (_book.SeriesId.HasValue)
            {
                _book.SeriesName.Should().Be(db.GetSeriesById(expected.SeriesId.Value).Name);
                _book.SeriesIndex.Should().Be(expected.SeriesIndex);
            }
            return this;
        }

        internal BookAssert ShouldBeSameAs(BookView expected, IDbConnection db)
        {
            _book.Should().NotBeNull();
            _book.Title.Should().Be(expected.Title);
            _book.Description.Should().Be(expected.Description);
            _book.Language.Should().Be((int)expected.Language);

            _book.IsPublic.Should().Be(expected.IsPublic);
            _book.IsPublished.Should().Be(expected.IsPublished);
            _book.Copyrights.Should().Be(expected.Copyrights);
            _book.DateAdded.Should().BeCloseTo(expected.DateAdded);
            _book.DateUpdated.Should().BeCloseTo(expected.DateUpdated);
            _book.Status.Should().Be(expected.Status);
            _book.YearPublished.Should().Be(expected.YearPublished);
            _book.AuthorId.Should().Be(expected.AuthorId);
            _book.AuthorName.Should().Be(db.GetAuthorById(expected.AuthorId).Name);
            _book.SeriesId.Should().Be(expected.SeriesId);
            _book.SeriesName.Should().Be(db.GetSeriesById(expected.SeriesId.Value).Name);
            _book.SeriesIndex.Should().Be(expected.SeriesIndex);

            _book.Categories.Should().HaveSameCount(expected.Categories);

            return this;
        }

        internal BookAssert ShouldBeSameCategories(IEnumerable<CategoryDto> expectedCategories)
        {
            expectedCategories.Should().HaveSameCount(_book.Categories);
            foreach (var expectedCategory in expectedCategories)
            {
                var category = _book.Categories.SingleOrDefault(c => c.Id == expectedCategory.Id);
                category.Should().NotBeNull();
                category.ShouldMatch(expectedCategory);
            }
            return this;
        }

        internal static void ShouldHaveDeletedBook(int id, IDbConnection databaseConnection)
        {
            databaseConnection.GetBookById(id).Should().BeNull();
        }

        internal static void ShouldHaveDeletedBookImage(int id, IDbConnection databaseConnection)
        {
            var image = databaseConnection.GetBookImage(id);
            image.Should().BeNull();
        }

        internal static void ShouldNotHaveUpdatedBookImage(int bookId, byte[] oldImage, IDbConnection dbConnection, IFileStorage fileStorage)
        {
            var imageUrl = dbConnection.GetBookImageUrl(bookId);
            imageUrl.Should().NotBeNull();
            var image = fileStorage.GetFile(imageUrl, CancellationToken.None).Result;
            image.Should().Equal(oldImage);
        }

        internal static void ShouldHaveAddedBookImage(int bookId, IDbConnection dbConnection, IFileStorage fileStorage)
        {
            var imageUrl = dbConnection.GetBookImageUrl(bookId);
            imageUrl.Should().NotBeNull();
            var image = fileStorage.GetFile(imageUrl, CancellationToken.None).Result;
            image.Should().NotBeNullOrEmpty();
        }

        internal static void ShouldHaveUpdatedBookImage(int bookId, byte[] newImage, IDbConnection dbConnection, IFileStorage fileStorage)
        {
            var imageUrl = dbConnection.GetBookImageUrl(bookId);
            imageUrl.Should().NotBeNull();
            var image = fileStorage.GetFile(imageUrl, CancellationToken.None).Result;
            image.Should().NotBeNull().And.Equal(newImage);
        }
    }

    public static class BookAssertionExtensions
    {
        public static BookAssert ShouldMatch(this BookView view, BookDto dto, IDbConnection dbConnection)
        {
            return new BookAssert(view)
                               .ShouldBeSameAs(dto, dbConnection);
        }
    }
}

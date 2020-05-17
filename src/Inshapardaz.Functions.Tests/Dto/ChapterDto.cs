﻿namespace Inshapardaz.Functions.Tests.Dto
{
    public class ChapterDto
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public int BookId { get; set; }

        public int ChapterNumber { get; set; }
    }

    public class ChapterContentDto
    {
        public int Id { get; set; }

        public int ChapterId { get; set; }

        public int FileId { get; set; }

        public string Language { get; set; }
    }
}

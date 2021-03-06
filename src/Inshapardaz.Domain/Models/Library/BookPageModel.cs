﻿using System;

namespace Inshapardaz.Domain.Models.Library
{
    public class BookPageModel
    {
        public string Text { get; set; }
        public int SequenceNumber { get; set; }
        public int BookId { get; set; }
        public int? ImageId { get; set; }
        public PageStatuses Status { get; set; }
        public int? AccountId { get; set; }
        public string AccountName { get; set; }
        public DateTime? AssignTimeStamp { get; set; }
        public int? ChapterId { get; set; }

        public string ChapterTitle { get; set; }
    }
}

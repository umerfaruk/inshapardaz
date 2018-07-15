﻿using System.Collections.Generic;
using Inshapardaz.Domain.Entities;
using Inshapardaz.Domain.Entities.Dictionary;

namespace Inshapardaz.Ports.Elasticsearch.Entities
{
    public class Word
    {
        public long Id { get; set; }

        public string Title { get; set; }

        public string TitleWithMovements { get; set; }

        public string Description { get; set; }

        public GrammaticalType Attributes { get; set; }

        public Languages Language { get; set; }

        public string Pronunciation { get; set; }

        public int DictionaryId { get; set; }

        public ICollection<Meaning> Meaning { get; set; } = new List<Meaning>();

        public ICollection<Translation> Translation { get; set; } = new List<Translation>();

        public ICollection<WordRelation> Relations { get; set; } = new List<WordRelation>();
    }
}
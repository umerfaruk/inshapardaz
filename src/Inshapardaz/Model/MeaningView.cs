﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MeaningModel.cs" company="Inshapardaz">
//   Muhammad Umer Farooq
// </copyright>
// <summary>
//   Defines the MeaningModel type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;

namespace Inshapardaz.Model
{
    public class MeaningView
    {
        public int Id { get; set; }

        public string Context { get; set; }

        public string Value { get; set; }

        public string Example { get; set; }

        public int WordDetailId { get; set; }

        public IEnumerable<LinkView> Links { get; set; }
    }
}
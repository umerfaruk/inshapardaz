﻿using Inshapardaz.Domain.Database.Entities;

namespace Inshapardaz.Domain.Commands
{
    public class UpdateDictionaryCommand : Command
    {
        public Dictionary Dictionary { get; set; }
    }
}

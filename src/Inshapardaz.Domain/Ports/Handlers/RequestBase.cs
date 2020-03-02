﻿using System;
using System.Security.Claims;
using Paramore.Brighter;

namespace Inshapardaz.Domain.Ports
{
    public abstract class RequestBase : IRequest
    {
        public Guid Id { get; set; }
    }

    public abstract class AuthoriseRequestBase : RequestBase
    {
        public AuthoriseRequestBase(ClaimsPrincipal claims)
        {
            Claims = claims;
        }

        public ClaimsPrincipal Claims { get; }
    }
}

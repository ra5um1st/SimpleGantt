﻿using System;
using SimpleGantt.Domain.ValueObjects;

namespace SimpleGantt.Domain.Entities;

public record ConnectionType : DomainType
{
    public ConnectionType(Guid Id, EntityName Name) : base(Id, Name)
    {
    }
}

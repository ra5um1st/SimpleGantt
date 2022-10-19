using System;
using SimpleGantt.Domain.Interfaces;
using SimpleGantt.Domain.ValueObjects;

namespace SimpleGantt.Domain.Entities;

public record DomainType(EntityName Name) : IEntity, INamable
{
    public Guid Id { get; }
}

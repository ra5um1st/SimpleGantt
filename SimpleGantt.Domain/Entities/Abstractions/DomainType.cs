using SimpleGantt.Domain.Interfaces;
using SimpleGantt.Domain.ValueObjects;

namespace SimpleGantt.Domain.Entities;

public record DomainType(EntityName Name) : IEntity<long>, INamable
{
    public long Id { get; protected set; }
}
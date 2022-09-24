using SimpleGantt.Domain.ValueObjects;

namespace SimpleGantt.Domain.Entities.Abstractions;

public record NamedDomainType : DomainType
{
    public EntityName Name { get; init; }

    public NamedDomainType(long Id, EntityName name) : base(Id)
    {
        Name = name;
    }
}

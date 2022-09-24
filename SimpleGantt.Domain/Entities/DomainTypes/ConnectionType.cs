using SimpleGantt.Domain.Entities.Abstractions;
using SimpleGantt.Domain.ValueObjects;

namespace SimpleGantt.Domain.Entities.DomainTypes;

public record ConnectionType : DomainType
{
    public EntityName Name { get; set; }

    public ConnectionType(long id, string name) : base(id)
    {
        Name = name;
    }
}

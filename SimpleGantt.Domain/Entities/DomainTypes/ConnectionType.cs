using SimpleGantt.Domain.Entities.Base;

namespace SimpleGantt.Domain.Entities.DomainTypes;

public record ConnectionType : DomainType
{
    public string Name { get; set; }

    public ConnectionType(long id, string name) : base(id)
    {
        Name = name;
    }
}

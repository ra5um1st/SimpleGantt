using SimpleGantt.Domain.Entities.Abstractions;

namespace SimpleGantt.Domain.Entities.DomainTypes;

public record UnitTimeType : NamedDomainType
{
    public UnitTimeType(long Id, string name) : base(Id, name)
    {
    }
}
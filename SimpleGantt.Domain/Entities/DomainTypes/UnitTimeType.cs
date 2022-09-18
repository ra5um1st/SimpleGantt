using SimpleGantt.Domain.Entities.Base;

namespace SimpleGantt.Domain.Entities.DomainTypes;

public record UnitTimeType : NamedDomainType
{
    public UnitTimeType(long Id, string name) : base(Id, name)
    {
    }
}
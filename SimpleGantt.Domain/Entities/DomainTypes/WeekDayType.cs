using SimpleGantt.Domain.Entities.Abstractions;

namespace SimpleGantt.Domain.Entities.DomainTypes;

public record WeekDayType : NamedDomainType
{
    public WeekDayType(long Id, string name) : base(Id, name)
    {
    }
}

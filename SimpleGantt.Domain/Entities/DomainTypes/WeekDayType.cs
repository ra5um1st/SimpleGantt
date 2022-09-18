using SimpleGantt.Domain.Entities.Base;

namespace SimpleGantt.Domain.Entities.DomainTypes;

public record WeekDayType : NamedDomainType
{
    public WeekDayType(long Id, string name) : base(Id, name)
    {
    }
}

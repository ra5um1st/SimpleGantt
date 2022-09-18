using SimpleGantt.Domain.Entities.Base;

namespace SimpleGantt.Domain.Entities.DomainTypes;

public record SceduleDayType : DomainType
{
    public WeekDayType WeekDayType { get; init; }
    public bool IsWorkingDay { get; init; }

    public SceduleDayType(long Id, WeekDayType weekDayType, bool isWorkingDay) : base(Id)
    {
        WeekDayType = weekDayType;
        IsWorkingDay = isWorkingDay;
    }
}

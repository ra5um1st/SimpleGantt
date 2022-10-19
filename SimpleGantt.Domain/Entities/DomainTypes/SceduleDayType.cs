using SimpleGantt.Domain.ValueObjects;

namespace SimpleGantt.Domain.Entities;

public record SceduleDayType(EntityName Name, DayOfWeekType WeekDayType, bool IsWorkingDay) : DomainType(Name);
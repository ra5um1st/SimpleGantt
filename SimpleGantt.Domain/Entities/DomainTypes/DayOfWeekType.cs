using SimpleGantt.Domain.ValueObjects;

namespace SimpleGantt.Domain.Entities;

public record DayOfWeekType(EntityName Name) : DomainType(Name);

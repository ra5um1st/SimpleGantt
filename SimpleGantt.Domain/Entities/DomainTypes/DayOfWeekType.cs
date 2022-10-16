using System;
using SimpleGantt.Domain.ValueObjects;

namespace SimpleGantt.Domain.Entities;

public record DayOfWeekType : DomainType
{
    public DayOfWeekType(Guid id, EntityName name) : base(id, name)
    {
    }
}

using System;
using SimpleGantt.Domain.ValueObjects;

namespace SimpleGantt.Domain.Events.Common;

public static class DomainEvents
{
    public abstract record NameChanged
    (
        Guid EntityId,
        EntityName NewName
    ) : DomainEvent;
}

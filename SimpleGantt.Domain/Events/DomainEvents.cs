using System;
using SimpleGantt.Domain.ValueObjects;

namespace SimpleGantt.Domain.Events;

public static class DomainEvents
{
    public abstract record NameChanged
    (
        Guid EntityId,
        EntityName NewName
    ) : DomainEvent;

    public abstract record EntityRemoved
    (
        Guid RemovedEntityId
    ) : DomainEvent;
}

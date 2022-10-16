using System;
using SimpleGantt.Domain.ValueObjects;

namespace SimpleGantt.Domain.Events.Common;

public static class DomainEvents
{
    public record NameChanged
    (
        Guid EntityId,
        EntityName NewEntityName
    ) : DomainEvent;
}

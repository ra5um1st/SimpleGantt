using System;
using System.Collections.Generic;
using SimpleGantt.Domain.Entities.Interfaces;

namespace SimpleGantt.Domain.Entities.Abstractions;

public record DomainType(long Id) : ISupportDomainEvents
{
    protected readonly HashSet<EventArgs> _domainEvents = new();
    public IReadOnlyCollection<EventArgs> DomainEvents => _domainEvents;

    public bool AddDomainEvent(EventArgs @event)
    {
        if (@event == null)
        {
            throw new ArgumentNullException(nameof(@event));
        }

        return _domainEvents.Add(@event);
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}

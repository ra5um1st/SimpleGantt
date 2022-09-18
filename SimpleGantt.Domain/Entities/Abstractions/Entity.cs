using System;
using System.Collections.Generic;
using SimpleGantt.Domain.Events.Abstractions;
using SimpleGantt.Domain.Interfaces;

namespace SimpleGantt.Domain.Entities.Abstractions;

public abstract class Entity : ISupportDomainEvents
{
    protected readonly HashSet<DomainEvent> _domainEvents = new();

    public long Id { get; set; }
    public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents;

    public bool AddDomainEvent(DomainEvent @event)
    {
        if(@event == null)
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

using System;
using System.Collections.Generic;
using SimpleGantt.Domain.Events;

namespace SimpleGantt.Domain.Entities;

public abstract class AggregateRoot : Entity
{
    private readonly HashSet<DomainEvent> _domainEvents = new();

    public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents;
    public long Version { get; private set; } = -1;
    public bool IsRemoved { get; protected set; } = false;

    protected void AddDomainEvent(DomainEvent @event)
    {
        if (@event == null)
        {
            throw new ArgumentNullException(nameof(@event));
        }

        _domainEvents.Add(@event);
        Version++;
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

    protected abstract void When(DomainEvent @event);
}

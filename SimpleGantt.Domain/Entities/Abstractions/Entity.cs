using System;
using System.Collections.Generic;
using SimpleGantt.Domain.Events.Abstractions;
using SimpleGantt.Domain.Interfaces;

namespace SimpleGantt.Domain.Entities.Abstractions;

public abstract class Entity : IHasDomainEvents
{
    private readonly List<DomainEvent> _domainEvents = new();

    public long Id { get; protected set; }

    public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents;

    public void AddDomainEvent(DomainEvent @event)
    {
        if(@event == null)
        {
            throw new ArgumentNullException(nameof(@event));
        }
        
        _domainEvents.Add(@event);
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}

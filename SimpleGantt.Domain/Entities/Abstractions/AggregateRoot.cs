using System;
using System.Collections.Generic;
using SimpleGantt.Domain.Events;
using SimpleGantt.Domain.Exceptions;

namespace SimpleGantt.Domain.Entities;

public abstract class AggregateRoot : Entity
{
    private readonly HashSet<DomainEvent> _domainEvents = new();

    public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents;
    public long Version { get; private set; } = -1;
    public bool Removed { get; protected set; } = false;

    public AggregateRoot(Guid id) : base(id) { }

    public virtual void Remove()
    {
        if (Removed) throw new DomainException($"Entity with id {Id} has already removed");
        Removed = true;
    }

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

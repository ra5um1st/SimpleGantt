using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using SimpleGantt.Domain.Events;
using SimpleGantt.Domain.Exceptions;

namespace SimpleGantt.Domain.Entities;

public abstract class AggregateRoot : Entity
{
    private readonly HashSet<DomainEvent> _domainEvents = new();

    [NotMapped]
    public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents;

    [NotMapped]
    public long Version { get; protected set; } = -1;

    [NotMapped]
    public bool Removed { get; protected set; } = false;

    public AggregateRoot(Guid id) : base(id) { }

    public void Remove()
    {
        if (Removed) throw new DomainException($"Entity with id {Id} has already removed");

        Removed = true;
    }

    private void AddDomainEvent(DomainEvent @event)
    {
        if (@event == null)
        {
            throw new ArgumentNullException(nameof(@event));
        }

        _domainEvents.Add(@event);
        Version++;
    }

    public void ClearDomainEvents() => _domainEvents.Clear();

    protected void Apply(DomainEvent @event) => @event.Apply(this);
    protected object ApplyDomainEvent(DomainEvent @event)
    {
        var result = @event.Apply(this);
        AddDomainEvent(@event);
        return result;
    }
}

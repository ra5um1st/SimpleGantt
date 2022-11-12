using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using SimpleGantt.Domain.Events;
using SimpleGantt.Domain.Exceptions;

namespace SimpleGantt.Domain.Entities;

public abstract class AggregateRoot : Entity
{
    private readonly HashSet<DomainEvent> _domainEvents = new();
    public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents;
    public long Version { get; protected set; } = -1;
    public bool Removed { get; protected set; } = false;
    internal AggregateRoot(Guid id) : base(id) { }

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
        // TODO : return a some result struct. Not the object (boxing/unboxing).
        return result;
    }

    public static T RestoreFrom<T>(IEnumerable<DomainEvent> events) 
        where T : AggregateRoot, new()
    {
        // TODO : Find method to create aggregate without pulblic ctors
        var aggregate = new T();

        foreach (var @event in events)
        {
            aggregate.Apply(@event);
            aggregate.Version++;
        }

        return aggregate;
    }
}

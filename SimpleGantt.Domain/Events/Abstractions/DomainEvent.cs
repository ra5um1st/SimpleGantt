using System;
using MediatR;
using SimpleGantt.Domain.Entities.Abstractions;

namespace SimpleGantt.Domain.Events.Abstractions;

public abstract record DomainEvent : INotification
{
    public DateTimeOffset TimeStamp { get; }
    public long EntityId { get; }
    public string EntityTypeName { get; }

    public DomainEvent(Entity entity)
    {
        TimeStamp = DateTimeOffset.Now;
        EntityId = entity.Id;
        EntityTypeName = nameof(Entity);
    }
}

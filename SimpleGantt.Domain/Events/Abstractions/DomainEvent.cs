using System;
using MediatR;

namespace SimpleGantt.Domain.Events;

public abstract record DomainEvent : INotification
{
    public long Id { get; }
    public DateTimeOffset CreatedAt { get; } = DateTimeOffset.Now;
}

using System;
using MediatR;

namespace SimpleGantt.Domain.Events;

public abstract record DomainEvent : INotification
{
    public DateTimeOffset CreatedAt { get; } = DateTimeOffset.Now;

    internal abstract object Apply(object @object);
}

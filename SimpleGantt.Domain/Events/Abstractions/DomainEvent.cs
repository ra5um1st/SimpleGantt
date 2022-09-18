using System;
using MediatR;

namespace SimpleGantt.Domain.Events.Abstractions;

public abstract class DomainEvent : INotification
{
    public DateTimeOffset TimeStamp { get; }

    public DomainEvent()
    {
        TimeStamp = DateTimeOffset.Now;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using SimpleGantt.Domain.Entities;
using SimpleGantt.Domain.Events;
using Task = System.Threading.Tasks.Task;

namespace SimpleGantt.Domain;

public static class AggregateEventConverter
{
    public static Task<T> ToAggregateAsync<T>(this IEnumerable<EventInfo> eventStream, CancellationToken cancellationToken = default)
        where T : AggregateRoot, new()
    {
        // TODO : add async/parallel

        var selectedEventStream = eventStream.Select(item => (item.Data, item.EventType));
        return ToAggregateAsync<T>(selectedEventStream, cancellationToken);
    }

    public static Task<T> ToAggregateAsync<T>(this IEnumerable<(string Data, string EventType)> eventStream, CancellationToken cancellationToken = default)
    where T : AggregateRoot, new()
    {
        if (eventStream == null)
        {
            throw new ArgumentNullException(nameof(eventStream));
        }

        if (!eventStream.Any())
        {
            throw new ArgumentException("Event stream is empty", nameof(eventStream));
        }

        var domainEvents = eventStream.Select(AsDomainEvent);
        var aggregate = AggregateRoot.RestoreFrom<T>(domainEvents);
        return Task.FromResult(aggregate);
    }

    public static Task<IEnumerable<EventInfo>> ToEventStreamAsync(this AggregateRoot aggregate, CancellationToken cancellationToken = default)
    {
        // TODO : add async/parallel
        var eventSream = new List<EventInfo>();

        foreach (var domainEvent in aggregate.DomainEvents)
        {
            var data = JsonSerializer.Serialize(domainEvent, domainEvent.GetType());
            var eventStream = new EventInfo(
                Guid.NewGuid(),
                DateTimeOffset.Now,
                domainEvent.GetType().AssemblyQualifiedName!,
                aggregate.Id,
                aggregate.GetType().AssemblyQualifiedName!,
                aggregate.Version,
                data);
            eventSream.Add(eventStream);
        }

        return Task.FromResult(eventSream.AsEnumerable());
    }

    public static DomainEvent AsDomainEvent(this EventInfo eventInfo) => AsDomainEvent((eventInfo.Data, eventInfo.EventType));

    public static DomainEvent AsDomainEvent(this (string Data, string EventType) eventInfo)
    {
        var eventType = Type.GetType(eventInfo.EventType)!;
        var domainEvent = (DomainEvent)JsonSerializer.Deserialize(eventInfo.Data, eventType)!;
        return domainEvent;
    }
}

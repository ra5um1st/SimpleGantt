using System;
using SimpleGantt.Domain.ValueObjects;

namespace SimpleGantt.Domain.Entities;

public class EventInfo : Entity
{
    // TODO : Create value object for data (json)
    public DateTimeOffset CreteadAt { get; set; }
    public string EventType { get; private set; }
    public Guid AggregateId { get; set; }
    public string AggregateType { get; set; }
    public long Version { get; set; }
    public string Data { get; set; }

    public EventInfo(Guid id, DateTimeOffset creteadAt, string eventType, Guid aggregateId, string aggregateType, long version, string data) : base(id)
    {
        CreteadAt = creteadAt;
        EventType = eventType;
        AggregateId = aggregateId;
        AggregateType = aggregateType;
        Version = version;
        Data = data;
    }
}

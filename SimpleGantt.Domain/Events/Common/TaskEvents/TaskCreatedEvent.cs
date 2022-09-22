using System;
using SimpleGantt.Domain.Events.Abstractions;
using SimpleGantt.Domain.ValueObjects;

namespace SimpleGantt.Domain.Events.Common.TaskEvents;

public class TaskCreatedEvent : DomainEvent
{
    public string Name { get; set; } = string.Empty;
    public DateTimeOffset StartDate { get; set; }
    public DateTimeOffset FinishDate { get; set; }
    public Percentage ComplitionPercentage { get; set; } = 0;
}

using System;
using SimpleGantt.Domain.Entities.Common;
using SimpleGantt.Domain.Events.Abstractions;
using SimpleGantt.Domain.ValueObjects;

namespace SimpleGantt.Domain.Events.Common.TaskEvents;

public record TaskCreatedEvent : DomainCreatedEvent<Task>
{
    public EntityName Name { get; }
    public DateTimeOffset StartDate { get; }
    public DateTimeOffset FinishDate { get; }
    public Percentage CompletionPercentage { get; }


    public TaskCreatedEvent(Task task) : base (task)
    {
        Name = task.Name;
        StartDate = task.StartDate;
        FinishDate = task.FinishDate;
        CompletionPercentage = task.CompletionPercentage;
    }
}

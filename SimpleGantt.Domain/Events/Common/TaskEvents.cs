using System;
using SimpleGantt.Domain.Entities;
using SimpleGantt.Domain.ValueObjects;

namespace SimpleGantt.Domain.Events.Common;

public static class TaskEvents
{
    public record TaskCreated
    (
        Guid ProjectId,
        Guid TaskId,
        EntityName Name,
        DateTimeOffset StartDate,
        DateTimeOffset FinishDate,
        Percentage CompletionPercentage
    ) : DomainEvent;

    public record TaskRemoved
    (
        Guid ProjectId,
        Guid TaskId
    ) : DomainEvent;

    public record StartDateChanged  
    (
        Guid TaskId,
        DateTimeOffset NewStartDate
    ) : DomainEvent;

    public record FinishDateChanged
    (
        Guid TaskId,
        DateTimeOffset NewFinishDate
    ) : DomainEvent;

    public record CompletionPercentageChanged
    (
        Guid TaskId,
        Percentage NewCompletionPercentage
    ) : DomainEvent;

    public record BothFinishDatesChanged
    (
        Guid MainTaskId,
        Guid ChildTaskId,
        DateTimeOffset NewFinishDate,
        DateTimeOffset NewChildFinishDate,
        ConnectionType ConnectionType
    ) : DomainEvent;

    public record BothStartDatesChanged
    (
        Guid MainTaskId,
        Guid ChildTaskId,
        DateTimeOffset NewStartDate,
        DateTimeOffset NewChildStartDate,
        ConnectionType ConnectionType
    ) : DomainEvent;

    public record FinishAndStartDatesChanged
    (
        Guid MainTaskId,
        Guid ChildTaskId,
        DateTimeOffset NewFinishDate,
        DateTimeOffset NewChildStartDate,
        ConnectionType ConnectionType
    ) : DomainEvent;

    public record StartAndFinishDatesChanged
    (
        Guid MainTaskId,
        Guid ChildTaskId,
        DateTimeOffset NewStartDate,
        DateTimeOffset NewChildFinishDate,
        ConnectionType ConnectionType
    ) : DomainEvent;
}
using System;
using SimpleGantt.Domain.Entities;
using SimpleGantt.Domain.ValueObjects;

namespace SimpleGantt.Domain.Events.Common;

public static class ProjectEvents
{
    public record TaskAddedToProject
    (
        Guid ProjectId,
        Guid TaskId,
        EntityName Name,
        DateTimeOffset StartDate,
        DateTimeOffset FinishDate,
        Percentage CompletionPercentage
    ) : DomainEvent;

    public record TaskRemovedFromProject
    (
        Guid ProjectId,
        Guid TaskId
    ) : DomainEvent;

    public record ResourceAddedToProject
    (
        Guid ProjectId,
        Guid ResourceId,
        EntityName Name,
        uint Count
    ) : DomainEvent;

    public record ResourceRemovedFromProject
    (
        Guid ProjectId,
        Guid ResourceId
    ) : DomainEvent;

    public record ConnectionAdded
    (
        Guid ConnectionId,
        Guid MainTaskId,
        Guid ChildTaskId,
        ConnectionType ConnectionType
    ) : DomainEvent;

    public record ConnectionRemoved
    (
        Guid ConnectionId,
        Guid MainTaskId,
        Guid ChildTaskId,
        ConnectionType ConnectionType
    ) : DomainEvent;

    public record SubtaskAdded
    (
        Guid HierarchyId,
        Guid MainTaskId,
        Guid ChildTaskId
    ) : DomainEvent;

    public record SubtaskRemoved
    (
        Guid HierarchyId,
        Guid MainTaskId,
        Guid ChildTaskId
    ) : DomainEvent;

    public record ProjectCreated
    (
        Guid ProjectId,
        EntityName Name,
        DateTimeOffset CreatedAt
    ) : DomainEvent;

    public record ProjectRemoved
    (
        Guid ProjectId
    ) : DomainEvent;
}

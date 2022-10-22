using System;
using System.Data.SqlTypes;
using SimpleGantt.Domain.Entities;
using SimpleGantt.Domain.ValueObjects;

namespace SimpleGantt.Domain.Events.Common;

public static class ProjectEvents
{
    public record ResourceCreated
    (
        Guid ProjectId,
        Guid ResourceId,
        EntityName Name,
        uint Count
    ) : DomainEvent;

    public record MaterialResourceAdded
    (
        Guid ProjectId,
        Guid ResourceId,
        EntityName Name,
        uint Count,
        SqlMoney Cost,
        CurrencyType CurrencyType
    ) : ResourceCreated(ProjectId, ResourceId, Name, Count);

    public record WorkingResourceAdded
    (
        Guid ProjectId,
        Guid ResourceId,
        EntityName Name,
        uint Count,
        WorkScedule WorkScedule,
        Salary Salary
    ) : ResourceCreated(ProjectId, ResourceId, Name, Count);

    public record ResourceRemoved
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
        DateTimeOffset ProjectCreatedAt
    ) : DomainEvent;

    public record ProjectRemoved
    (
        Guid ProjectId
    ) : DomainEvent;

    public record TaskResourceAdded
    (
        Guid TaskId,
        Guid ResourceId,
        Guid TaskResourceId,
        uint Count
    ) : DomainEvent;

    public record TaskResourceAmountChanged
    (
        Guid TaskId,
        Guid TaskResourceId,
        uint Count
    ) : DomainEvent;

    public record TaskResourceRemoved
    (
        Guid TaskId,
        Guid ResourceId,
        Guid TaskResourceId
    ) : DomainEvent;
}

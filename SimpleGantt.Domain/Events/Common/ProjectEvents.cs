using System;
using SimpleGantt.Domain.Entities;

namespace SimpleGantt.Domain.Events.Common;

public static class ProjectEvents
{
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
}

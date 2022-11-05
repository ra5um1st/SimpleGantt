using System;
using System.Linq;
using SimpleGantt.Domain.Entities.DomainTypes;
using SimpleGantt.Domain.Events;
using SimpleGantt.Domain.Exceptions;
using SimpleGantt.Domain.ValueObjects;
using static SimpleGantt.Domain.Events.DomainEvents;
using static SimpleGantt.Domain.Queries.CommonQueries;

namespace SimpleGantt.Domain.Entities;

public partial class Project
{
    public record TaskCreated
    (
        Guid ProjectId,
        Guid TaskId,
        EntityName Name,
        DateTimeOffset StartDate,
        DateTimeOffset FinishDate,
        Percentage CompletionPercentage
    ) : DomainEvent
    {
        internal override object Apply(object @object)
        {
            var project = (Project)@object;

            if (project.Id != ProjectId)
            {
                throw new DomainEventException(nameof(TaskCreated));
            }

            if (project.Tasks.Any(item => item.Id == TaskId))
            {
                throw new DomainExistsException(TaskId);
            }

            var task = new Task(TaskId, project, Name, StartDate, FinishDate, CompletionPercentage);
            var unused = project._tasks.Add(task);

            return task;
        }
    }

    public record TaskRemoved
    (
        Guid ProjectId,
        Guid TaskId
    ) : DomainEvent
    {
        internal override object Apply(object @object)
        {
            var project = (Project)@object;

            if (!project.Tasks.Any(item => item.Id == TaskId))
            {
                throw new DomainNotExistException(TaskId);
            }

            if (project.Id != ProjectId)
            {
                throw new DomainEventException(nameof(TaskRemoved));
            }

            var task = GetEntityById(project.Tasks, TaskId);
            var unused = project._tasks.Remove(task);

            return default!;
        }
    }

    public record TaskStartDateChanged
    (
        Guid TaskId,
        DateTimeOffset NewStartDate
    ) : DomainEvent
    {
        internal override object Apply(object @object)
        {
            var project = (Project)@object;
            var task = GetEntityById(project.Tasks, TaskId);
            task.ChangeStartDate(NewStartDate);

            return default!;
        }
    }

    public record TaskFinishDateChanged
    (
        Guid TaskId,
        DateTimeOffset NewFinishDate
    ) : DomainEvent
    {
        internal override object Apply(object @object)
        {
            var project = (Project)@object;
            var task = GetEntityById(project.Tasks, TaskId);
            task.ChangeFinishDate(NewFinishDate);

            return default!;
        }
    }

    public record TaskCompletionPercentageChanged
    (
        Guid TaskId,
        Percentage NewCompletionPercentage
    ) : DomainEvent
    {
        internal override object Apply(object @object)
        {
            // TODO: Add buisness rules for Completion Percentage change

            var project = (Project)@object;
            var task = GetEntityById(project.Tasks, TaskId);
            task.ChangeCompletionPercentage(NewCompletionPercentage);

            return default!;
        }
    }

    public record TaskConnectionAdded
    (
        Guid ConnectionId,
        Guid MainTaskId,
        Guid ChildTaskId,
        ConnectionType ConnectionType
    ) : DomainEvent
    {
        internal override object Apply(object @object)
        {
            var project = (Project)@object;
            var parent = GetEntityById(project.Tasks, MainTaskId);
            var child = GetEntityById(project.Tasks, ChildTaskId);
            var connection = parent.AddConnection(ConnectionId, child, ConnectionType);

            return connection;
        }
    }

    public record TaskConnectionRemoved
    (
        Guid ConnectionId,
        Guid MainTaskId
    ) : DomainEvent
    {
        internal override object Apply(object @object)
        {
            var project = (Project)@object;
            var parent = GetEntityById(project.Tasks, MainTaskId);
            parent.RemoveConnection(ConnectionId);

            return default!;
        }
    }

    public record SubtaskAdded
    (
        Guid MainTaskId,
        Guid ChildTaskId,
        Guid HierarchyId
    ) : DomainEvent
    {
        internal override object Apply(object @object)
        {
            var project = (Project)@object;
            var parent = GetEntityById(project.Tasks, MainTaskId);
            var child = GetEntityById(project.Tasks, ChildTaskId);
            var taskHierarchy = parent.AddSubtask(HierarchyId, child);

            return taskHierarchy;
        }
    }

    public record SubtaskRemoved
    (
        Guid MainTaskId,
        Guid HierarchyId
    ) : DomainEvent
    {
        internal override object Apply(object @object)
        {
            var project = (Project)@object;
            var parent = GetEntityById(project.Tasks, MainTaskId);
            parent.RemoveSubtask(HierarchyId);

            return default!;
        }
    }

    public record TaskNameChanged
    (
        Guid EntityId,
        EntityName NewName
    ) : NameChanged(EntityId, NewName)
    {
        internal override object Apply(object @object)
        {
            var project = (Project)@object;

            var task = GetEntityById(project.Tasks, EntityId);

            if (task.Name != NewName)
            {
                task.Name = NewName;
            }

            return default!;
        }
    }

    public record TaskResourceAdded
    (
        Guid TaskId,
        Guid ResourceId,
        Guid TaskResourceId,
        double Amount
    ) : DomainEvent
    {
        internal override object Apply(object @object)
        {
            var project = (Project)@object;
            var task = GetEntityById(project.Tasks, TaskId);
            var resource = GetEntityById(project.Resources, ResourceId);
            var taskResource = task.AddResource(TaskResourceId, resource, Amount);

            return taskResource;
        }
    }

    public record TaskResourceAmountAdded
    (
        Guid TaskId,
        Guid TaskResourceId,
        double Amount
    ) : DomainEvent
    {
        internal override object Apply(object @object)
        {
            var project = (Project)@object;
            var task = GetEntityById(project.Tasks, TaskId);
            var taskResource = task.Resources.First(item => item.Id == TaskResourceId);
            taskResource.AddResourceAmount(Amount);

            return default!;
        }
    }

    public record TaskResourceAmountRemoved
    (
        Guid TaskId,
        Guid TaskResourceId,
        double Amount
    ) : DomainEvent
    {
        internal override object Apply(object @object)
        {
            var project = (Project)@object;
            var task = GetEntityById(project.Tasks, TaskId);
            var taskResource = task.Resources.First(item => item.Id == TaskResourceId);
            taskResource.RemoveResourceAmount(Amount);

            return default!;
        }
    }

    public record TaskResourceRemoved
    (
        Guid TaskId,
        Guid TaskResourceId
    ) : DomainEvent
    {
        internal override object Apply(object @object)
        {
            var project = (Project)@object;
            var task = GetEntityById(project.Tasks, TaskId);
            task.RemoveResource(TaskResourceId);

            return default!;
        }
    }
}
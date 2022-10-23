using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using SimpleGantt.Domain.Exceptions;
using SimpleGantt.Domain.Interfaces;
using SimpleGantt.Domain.ValueObjects;

namespace SimpleGantt.Domain.Entities.Common;

public sealed class Task : Entity, INamable
{
    private readonly HashSet<TaskConnection> _connections = new();
    private readonly HashSet<TaskHierarchy> _hierarchy = new();
    private readonly HashSet<TaskResource> _resources = new();

    public EntityName Name { get; internal set; } = string.Empty;
    public DateTimeOffset StartDate { get; internal set; }
    public DateTimeOffset FinishDate { get; internal set; }
    public Percentage CompletionPercentage { get; internal set; } = 0;
    public Project Project { get; private set; }
    public IReadOnlyCollection<TaskConnection> Connections => _connections;
    public IReadOnlyCollection<TaskHierarchy> Hierarchy => _hierarchy;
    public IReadOnlyCollection<TaskResource> Resources => _resources;

    [NotMapped]
    public bool HasChilds => _connections.Any();

    [NotMapped]
    public bool HasConnections => _hierarchy.Any();

    [NotMapped]
    public bool IsMainTask => _hierarchy.Select(item => item.Child.Id != Id).Any();

    private Task() : base(Guid.Empty)
    {
        Project = null!;
    }

    internal Task(Guid id, Project project, EntityName name, DateTimeOffset startDate, DateTimeOffset finishData, Percentage percentage) : base(id)
    {
        Project = project ?? throw new ArgumentNullException(nameof(Project));
        Name = name;
        StartDate = startDate;
        FinishDate = finishData;
        CompletionPercentage = percentage;
    }

    internal void AddConnection(TaskConnection connection)
    {
        if (HasConnectionWithChild(connection.Child))
        {
            throw new DomainExistsException(nameof(connection));
        }

        _connections.Add(connection);
    }

    internal void RemoveConnection(TaskConnection connection)
    {
        if (!HasConnectionWithChild(connection.Child))
        {
            throw new DomainNotExistException(nameof(connection));
        }

        _connections.Remove(connection);
    }

    internal void AddTaskResource(TaskResource taskResource)
    {
        if (_resources.Contains(taskResource))
        {
            throw new DomainExistsException(nameof(taskResource));
        }

        _resources.Add(taskResource);
    }

    internal void RemoveTaskResource(TaskResource taskResource)
    {
        if (!_resources.Contains(taskResource))
        {
            throw new DomainNotExistException(nameof(taskResource));
        }

        _resources.Remove(taskResource);
    }

    public bool HasConnectionOfType(ConnectionType connectionType)
    {
        return _connections
                    .Select(item => item.ConnectionType.Name)
                    .Any(item => item == connectionType.Name);
    }

    public bool HasConnectionWithChild(Task child)
    {
        return _connections
                    .Select(item => item.Child.Id)
                    .Any(item => item == child.Id);
    }

    public bool HasConnectionWithChildOfType(Task child, ConnectionType connectionType)
    {
        return _connections
                    .Select(item => new { item.Child.Id, ConnectionTypeName = item.ConnectionType.Name })
                    .Any(item => item.Id == child.Id && item.ConnectionTypeName == connectionType.Name);
    }

    public bool HasConnectionWithParentOfTYpe(Task parent, ConnectionType connectionType)
    {
        return _connections
                    .Select(item => new { item.Parent.Id, ConnectionTypeName = item.ConnectionType.Name })
                    .Any(item => item.Id == parent.Id && item.ConnectionTypeName == connectionType.Name);
    }

    public TaskConnection GetConnectionByChild(Task child)
    {
        return Connections.First(
            item => item.Parent.Id == Id &&
            item.Child.Id == child.Id);
    }

    internal void AddHierarchy(TaskHierarchy hierarchy)
    {
        if (HasHierarchyWithChild(hierarchy.Child))
        {
            throw new DomainExistsException(nameof(hierarchy));
        }

        _hierarchy.Add(hierarchy);
    }

    internal void RemoveHierarchy(TaskHierarchy hierarchy)
    {
        if (!HasHierarchyWithChild(hierarchy.Child))
        {
            throw new DomainNotExistException(nameof(hierarchy));
        }

        _hierarchy.Remove(hierarchy);
    }

    public bool HasHierarchyWithChild(Task child)
    {
        return _hierarchy.Any(item => item.Parent.Id == Id && item.Child.Id == child.Id);
    }

    public TaskHierarchy GetHierarchyByChild(Task child)
    {
        return _hierarchy.First(
            item => item.Parent.Id == Id &&
            item.Child.Id == child.Id);
    }
}

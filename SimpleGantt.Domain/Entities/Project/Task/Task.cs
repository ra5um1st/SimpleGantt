using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using SimpleGantt.Domain.Entities.DomainTypes;
using SimpleGantt.Domain.Exceptions;
using SimpleGantt.Domain.Interfaces;
using SimpleGantt.Domain.Queries;
using SimpleGantt.Domain.ValueObjects;
using static SimpleGantt.Domain.Queries.CommonQueries;

namespace SimpleGantt.Domain.Entities;

// TODO: Extract task interface
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

    private Task() : base(default)
    {

    }

    internal Task(Guid id, Project project, EntityName name, DateTimeOffset startDate, DateTimeOffset finishData, Percentage percentage) : base(id)
    {
        Project = project ?? throw new ArgumentNullException(nameof(Project));
        Name = name;
        StartDate = startDate;
        FinishDate = finishData;
        CompletionPercentage = percentage;
    }

    internal TaskResource AddResource(Guid taskResourceId, Resource resource, double amount)
    {
        if (HasEntityWithId(_resources, taskResourceId))
        {
            throw new DomainExistsException(taskResourceId);
        }
        if (HasEntityWithId(_resources.Select(item => item.Resource), resource.Id))
        {
            throw new DomainExistsException(nameof(resource));
        }

        var taskResource = new TaskResource(taskResourceId, this, resource, amount);
        _resources.Add(taskResource);

        return taskResource;
    }

    internal void RemoveResource(Guid taskResourceId)
    {
        if (!HasEntityWithId(_resources, taskResourceId))
        {
            throw new DomainExistsException(taskResourceId);
        }

        var taskResource = GetEntityById(_resources, taskResourceId);
        _resources.Remove(taskResource);
    }

    internal TaskHierarchy AddSubtask(Guid hierarchyId, Task child)
    {
        if (this.HasSubtask(child)) throw new DomainExistsException(nameof(child));

        var taskHierarchy = new TaskHierarchy(hierarchyId, this, child);
        _hierarchy.Add(taskHierarchy);
        child._hierarchy.Add(taskHierarchy);

        return taskHierarchy;
    }

    internal void RemoveSubtask(Guid hierarchyId)
    {
        if (!HasEntityWithId(_hierarchy, hierarchyId))
        {
            throw new DomainNotExistException(hierarchyId);
        }

        var taskHierarchy = GetEntityById(_hierarchy, hierarchyId);
        _hierarchy.Remove(taskHierarchy);
        taskHierarchy.Child._hierarchy.Remove(taskHierarchy);
    }

    internal TaskConnection AddConnection(Guid connectionId, Task child, ConnectionType connectionType)
    {
        if (this.HasConnectionWithChild(child))
        {
            throw new DomainNotExistException(connectionId);
        }

        var connection = new TaskConnection(connectionId, this, child, connectionType);
        _connections.Add(connection);
        child._connections.Add(connection);

        return connection;
    }

    internal void RemoveConnection(Guid connectionId)
    {
        if (!HasEntityWithId(_connections, connectionId))
        {
            throw new DomainNotExistException(connectionId);
        }

        var connection = _connections.First(item => item.Id == connectionId);
        _connections.Remove(connection);
        connection.Child._connections.Remove(connection);
    }

    internal void ChangeStartDate(DateTimeOffset newStartDate)
    {
        if (StartDate == newStartDate) return;

        StartDate = newStartDate;
        var connections = Connections.Where(task => task.Parent.Id == Id);

        foreach (var connection in connections)
        {
            connection.ApplyConnection();
        }
    }

    internal void ChangeFinishDate(DateTimeOffset newFinishDate)
    {
        if (FinishDate == newFinishDate) return;

        FinishDate = newFinishDate;
        var connections = Connections.Where(task => task.Parent.Id == Id);

        foreach (var connection in connections)
        {
            connection.ApplyConnection();
        }
    }

    internal void ChangeCompletionPercentage(Percentage newPercentage)
    {
        if (CompletionPercentage != newPercentage) return;

        CompletionPercentage = newPercentage;

        // TODO : add logic to perform copletion percentage chagnde correctly
    }
}

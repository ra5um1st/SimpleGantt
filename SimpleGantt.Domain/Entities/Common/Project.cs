using System;
using System.Collections.Generic;
using System.Linq;
using SimpleGantt.Domain.Events;
using SimpleGantt.Domain.Exceptions;
using SimpleGantt.Domain.Interfaces;
using SimpleGantt.Domain.ValueObjects;
using static SimpleGantt.Domain.Events.Common.ProjectEvents;

namespace SimpleGantt.Domain.Entities.Common;

public class Project : AggregateRoot, INamable, ITrackable
{
    #region Fields

    private readonly HashSet<Task> _tasks = new();
    private readonly HashSet<Resource> _resources = new();

    #endregion

    #region Properties

    public EntityName Name { get; private set; } = string.Empty;
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset UpdatedAt { get; private set; }
    public IReadOnlyCollection<Task> Tasks => _tasks;
    public IReadOnlyCollection<Resource> Resources => _resources;

    #endregion

    #region Constructors

    private Project() { }

    public Project(EntityName name, DateTimeOffset createdAt)
    {
        Name = name;
        CreatedAt = createdAt;
        UpdatedAt = createdAt;
    }

    #endregion

    #region Methods

    public Task GetTaskById(Guid taskId)
    {
        return _tasks.First(item => item.Id == taskId);
    }

    public void AddConnection(Task parent, Task child, ConnectionType connectionType)
    {
        if (parent.HasConnectionWithChild(child))
        {
            throw new DomainExistsException(parent, child);
        }

        var connection = new TaskConnection(parent, child, connectionType);
        parent.AddConnection(connection);
        child.AddConnection(connection);
        AddDomainEvent(new ConnectionAdded(connection.Id, connection.Id, connection.Id, connection.ConnectionType));
    }

    public void RemoveConnection(Task parent, Task child)
    {
        if (!parent.HasConnectionWithChild(child))
        {
            throw new DomainNotExistException(parent, child);
        }

        var connection = parent.TaskConnections.First(item => item.Parent.Id == Id && item.Child.Id == child.Id);
        parent.RemoveConnection(connection);
        child.RemoveConnection(connection);
        AddDomainEvent(new ConnectionRemoved(connection.Id, connection.Id, connection.Id, connection.ConnectionType));
    }

    protected override void When(DomainEvent @event)
    {
        switch (@event)
        {
            case ConnectionAdded connectionAdded:
            {
                var parent = GetTaskById(connectionAdded.MainTaskId);
                var child = GetTaskById(connectionAdded.ChildTaskId);
                var connection = new TaskConnection(parent, child, connectionAdded.ConnectionType);
                parent.AddConnection(connection);
                child.AddConnection(connection);
                break;
            }
            case ConnectionRemoved connectionRemoved:
            {
                var parent = GetTaskById(connectionRemoved.MainTaskId);
                var child = GetTaskById(connectionRemoved.ChildTaskId);
                var connection = parent.GetConnectionByChild(child);
                parent.RemoveConnection(connection);
                child.RemoveConnection(connection);
                break;
            }
            case SubtaskAdded subtaskAdded:
            {
                var parent = GetTaskById(subtaskAdded.MainTaskId);
                var child = GetTaskById(subtaskAdded.ChildTaskId);
                var hierarchy = new TaskHierarchy(parent, child);
                parent.AddHierarchy(hierarchy);
                child.AddHierarchy(hierarchy);
                break;
            }
            case SubtaskRemoved subtasRemoved:
            {
                var parent = GetTaskById(subtasRemoved.MainTaskId);
                var child = GetTaskById(subtasRemoved.ChildTaskId);
                var hierarchy = parent.GetHierarchyByChild(child);
                parent.RemoveHierarchy(hierarchy);
                child.RemoveHierarchy(hierarchy);
                break;
            }
            default:
                break;
        }
    }

    #endregion
}

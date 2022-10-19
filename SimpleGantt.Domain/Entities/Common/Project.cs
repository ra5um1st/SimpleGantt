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

        AddDomainEvent(new ProjectCreated(Id, Name, CreatedAt));
    }

    #endregion

    #region Methods

    public void AddTask(Task task)
    {
        _tasks.Add(task);
        AddDomainEvent(new TaskAddedToProject(Id, task.Id, task.Name, task.StartDate, task.FinishDate, task.CompletionPercentage));
    }

    public void RemoveTask(Task task)
    {
        _tasks.Remove(task);
        AddDomainEvent(new TaskRemovedFromProject(Id, task.Id));
    }

    public void AddResource(Resource resource)
    {
        _resources.Add(resource);
        AddDomainEvent(new ResourceAddedToProject(Id, resource.Id, resource.Name, resource.Count));
    }
    public void RemoveResource(Resource resource)
    {
        _resources.Remove(resource);
        AddDomainEvent(new ResourceRemovedFromProject(Id, resource.Id));
    }

    public Task GetTaskById(Guid taskId)
    {
        return _tasks.First(item => item.Id == taskId);
    }

    private void ApplyConnection(Task parent, Task child, ConnectionType connectionType)
    {
        switch (connectionType.Name)
        {
            case ConnectionType.StartStart:
            {
                if (child.StartDate >= parent.StartDate) return;

                var difference = parent.StartDate - child.StartDate;

                if (difference > TimeSpan.Zero)
                {
                    child.ChangeStartDate(parent.StartDate);
                    child.ChangeFinishDate(parent.FinishDate + difference);
                }
                else
                {
                    parent.ChangeStartDate(child.StartDate);
                    parent.ChangeFinishDate(child.FinishDate + difference);
                }

                break;
            }
            case ConnectionType.StartFinish:
            {
                if (child.FinishDate >= parent.StartDate) return;

                var difference = parent.StartDate - child.FinishDate;

                if (difference > TimeSpan.Zero)
                {
                    child.ChangeFinishDate(parent.StartDate);
                    child.ChangeStartDate(parent.StartDate + difference);
                }
                else
                {
                    parent.ChangeStartDate(child.FinishDate);
                    parent.ChangeFinishDate(parent.FinishDate + difference);
                }

                break;
            }
            case ConnectionType.FinishStart:
            {
                if (child.StartDate >= parent.FinishDate) return;

                var difference = parent.FinishDate - child.StartDate;

                if (difference > TimeSpan.Zero)
                {
                    child.ChangeStartDate(parent.FinishDate);
                    child.ChangeFinishDate(child.FinishDate + difference);
                }
                else
                {
                    parent.ChangeStartDate(parent.StartDate + difference);
                    parent.ChangeFinishDate(child.StartDate);
                }

                break;
            }
            case ConnectionType.FinishFinish:
            {
                if (child.StartDate >= parent.FinishDate) return;

                var difference = parent.FinishDate - child.FinishDate;

                if (difference > TimeSpan.Zero)
                {
                    child.ChangeStartDate(child.StartDate + difference);
                    child.ChangeFinishDate(parent.FinishDate);
                }
                else
                {
                    parent.ChangeStartDate(parent.StartDate + difference);
                    parent.ChangeFinishDate(child.StartDate);
                }

                break;
            }
            default:
                throw new DomainException($"Cannot apply connection to {parent.Name} and {child.Name} of type {connectionType.Name}");
        }
    }

    public void AddConnection(Task parent, Task child, ConnectionType connectionType)
    {
        if (parent.HasConnectionWithChild(child))
        {
            throw new EntityExistsException(nameof(child));
        }

        var connection = new TaskConnection(parent, child, connectionType);
        parent.AddConnection(connection);
        child.AddConnection(connection);
        ApplyConnection(connection.Parent, connection.Child, connection.ConnectionType);
        AddDomainEvent(new ConnectionAdded(connection.Id, connection.Id, connection.Id, connection.ConnectionType));
    }

    public void RemoveConnection(Task parent, Task child)
    {
        if (!parent.HasConnectionWithChild(child))
        {
            throw new DomainNotExistException(nameof(child));
        }

        var connection = parent.TaskConnections.First(item => item.Parent.Id == Id && item.Child.Id == child.Id);
        parent.RemoveConnection(connection);
        child.RemoveConnection(connection);
        ApplyConnection(connection.Parent, connection.Child, connection.ConnectionType);
        AddDomainEvent(new ConnectionRemoved(connection.Id, connection.Id, connection.Id, connection.ConnectionType));
    }

    protected override void When(DomainEvent @event)
    {
        switch (@event)
        {
            case TaskAddedToProject taskAdded:
            {
                var task = new Task(this, taskAdded.Name, taskAdded.StartDate, taskAdded.FinishDate, taskAdded.CompletionPercentage);
                _tasks.Add(task);
                break;
            }
            case TaskRemovedFromProject taskRemoved:
            {
                var task = _tasks.First(item => item.Id == taskRemoved.TaskId);
                _tasks.Remove(task);
                task.Remove();
                break;
            }
            case ResourceAddedToProject resourceAdded:
            {
                var resource = new Resource(resourceAdded.Name, resourceAdded.Count);
                _resources.Add(resource);
                break;
            }
            case ResourceRemovedFromProject resourceRemoved:
            {
                var resource = _resources.First(item => item.Id == resourceRemoved.ResourceId);
                _resources.Remove(resource);
                break;
            }
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
            case ProjectCreated projectCreated:
            {
                Name = projectCreated.Name;
                CreatedAt = projectCreated.CreatedAt;
                break;
            }
            case ProjectRemoved projectRemoved:
            {
                Removed = true;
                break;
            }
            default:
                break;
        }
    }

    public override void Remove()
    {
        Removed = true;
        AddDomainEvent(new ProjectRemoved(Id));
    }

    #endregion
}

using System;
using System.Collections.Generic;
using System.Linq;
using SimpleGantt.Domain.Events;
using SimpleGantt.Domain.Exceptions;
using SimpleGantt.Domain.Interfaces;
using SimpleGantt.Domain.Queries;
using SimpleGantt.Domain.ValueObjects;
using static SimpleGantt.Domain.Events.Common.ProjectEvents;
using static SimpleGantt.Domain.Events.Common.TaskEvents;
using static SimpleGantt.Domain.Queries.ProjectQueries;
using static SimpleGantt.Domain.Queries.CommonQueries;

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

    private Project() : base(Guid.Empty) { }

    public Project(Guid id, EntityName name, DateTimeOffset createdAt) : base(id)
    {
        Name = name;
        CreatedAt = createdAt;
        UpdatedAt = createdAt;

        AddDomainEvent(new ProjectCreated(Id, Name, CreatedAt));
    }

    #endregion

    #region Methods

    public Task CreateTask(TaskCreated taskCreated)
    {
        var task = new Task(taskCreated.TaskId, this, taskCreated.Name, taskCreated.StartDate, taskCreated.FinishDate, taskCreated.CompletionPercentage);
        _tasks.Add(task);
        AddDomainEvent(taskCreated);

        return task;
    }

    public void RemoveTask(TaskRemoved taskRemoved)
    {
        var task = GetEntityById(_tasks, taskRemoved.TaskId);
        task.Remove();
        _tasks.Remove(task);
        AddDomainEvent(taskRemoved);
    }

    public Resource CreateResource(ResourceCreated resourceCreated)
    {
        var resource = new Resource(resourceCreated.ResourceId, this, resourceCreated.Name, resourceCreated.Count);
        _resources.Add(resource);
        AddDomainEvent(resourceCreated);

        return resource;
    }

    public void RemoveResource(Resource resource)
    {
        _resources.Remove(resource);
        AddDomainEvent(new ResourceRemoved(Id, resource.Id));
    }

    public TaskResource AddResourceToTask(TaskResourceAdded taskResourceAdded)
    {
        var task = GetEntityById(_tasks, taskResourceAdded.TaskId);
        var resource = GetEntityById(_resources, taskResourceAdded.ResourceId);
        var alreadyAdded = this.GetUsedResourcesAmountById(taskResourceAdded.ResourceId);

        if (taskResourceAdded.Count > resource.Count - alreadyAdded)
        {
            throw new DomainException($"Cannot add resource to task in the amount of {taskResourceAdded.Count}. " +
                $"To the project tasks have already been added {alreadyAdded}");
        }

        var taskResource = new TaskResource(taskResourceAdded.TaskResourceId, task, resource, taskResourceAdded.Count);
        task.AddTaskResource(taskResource);
        AddDomainEvent(taskResourceAdded);

        return taskResource;
    }

    public void ChangeTaskResourceAmount(TaskResourceAmountChanged amountChanged)
    {
        if (amountChanged.Count == 0) return;

        var task = GetEntityById(_tasks, amountChanged.TaskId);
        var resource = GetEntityById(_resources, amountChanged.TaskResourceId);
        var taskResource = task.Resources.First(item => item.Id == amountChanged.TaskResourceId);
        var alreadyAdded = this.GetUsedResourcesAmountById(amountChanged.TaskResourceId);

        if (amountChanged.Count > 0 && amountChanged.Count > resource.Count - alreadyAdded)
        {
            throw new DomainException("Cannot change task resource amount. Have not resource amount enought.");
        }

        taskResource.ChangeResourceAmount(amountChanged.Count);
        AddDomainEvent(amountChanged);
    }

    public TaskResource RemoveResourceFromTask(TaskResourceRemoved taskResourceRemoved)
    {
        var task = GetEntityById(_tasks, taskResourceRemoved.TaskId);
        var resource = GetEntityById(_tasks, taskResourceRemoved.ResourceId);
        var taskResource = task.Resources.First(item => item.Id == taskResourceRemoved.TaskResourceId);
        task.RemoveTaskResource(taskResource);
        AddDomainEvent(taskResourceRemoved);

        return taskResource;
    }

    private void ApplyConnection(Task parent, Task child, ConnectionType connectionType)
    {
        switch (connectionType)
        {
            case nameof(ConnectionType.StartStart):
            {
                if (child.StartDate >= parent.StartDate) return;

                var difference = parent.StartDate - child.StartDate;

                if (difference > TimeSpan.Zero)
                {
                    child.ChangeStartDate(parent.StartDate);
                    child.ChangeFinishDate(child.FinishDate + difference);
                }
                else
                {
                    parent.ChangeStartDate(child.StartDate);
                    parent.ChangeFinishDate(child.FinishDate + difference);
                }

                break;
            }
            case nameof(ConnectionType.StartFinish):
            {
                if (child.FinishDate >= parent.StartDate) return;

                var difference = parent.StartDate - child.FinishDate;

                if (difference > TimeSpan.Zero)
                {
                    child.ChangeFinishDate(parent.StartDate);
                    child.ChangeStartDate(child.StartDate + difference);
                }
                else
                {
                    parent.ChangeStartDate(child.FinishDate);
                    parent.ChangeFinishDate(parent.FinishDate + difference);
                }

                break;
            }
            case nameof(ConnectionType.FinishStart):
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
            case nameof(ConnectionType.FinishFinish):
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

    public TaskConnection AddConnection(ConnectionAdded connectionAdded)
    {
        var parent = GetEntityById(_tasks, connectionAdded.MainTaskId);
        var child = GetEntityById(_tasks, connectionAdded.ChildTaskId);

        if (parent.HasConnectionWithChild(child))
        {
            throw new DomainExistsException(nameof(child));
        }

        var connection = new TaskConnection(connectionAdded.ConnectionId, parent, child, connectionAdded.ConnectionType);
        parent.AddConnection(connection);
        child.AddConnection(connection);
        ApplyConnection(connection.Parent, connection.Child, connection.ConnectionType);
        AddDomainEvent(connectionAdded);

        return connection;
    }

    public void RemoveConnection(ConnectionRemoved connectionRemoved)
    {
        var parent = GetEntityById(_tasks, connectionRemoved.MainTaskId);
        var child = GetEntityById(_tasks, connectionRemoved.ChildTaskId);

        if (!parent.HasConnectionWithChild(child))
        {
            throw new DomainNotExistException(nameof(child));
        }

        var connection = parent.Connections.First(item => item.Parent.Id == Id && item.Child.Id == child.Id);
        parent.RemoveConnection(connection);
        child.RemoveConnection(connection);
        ApplyConnection(connection.Parent, connection.Child, connection.ConnectionType);
        AddDomainEvent(connectionRemoved);
    }

    protected override void When(DomainEvent @event)
    {
        switch (@event)
        {
            case TaskCreated taskCreated:
            {
                var task = new Task(taskCreated.TaskId, this, taskCreated.Name, taskCreated.StartDate, taskCreated.FinishDate, taskCreated.CompletionPercentage);
                _tasks.Add(task);
                break;
            }
            case TaskRemoved taskRemoved:
            {
                var task = _tasks.First(item => item.Id == taskRemoved.TaskId);
                _tasks.Remove(task);
                task.Remove();
                break;
            }
            case ResourceCreated resourceCreated:
            {
                var resource = new Resource(resourceCreated.ResourceId, this, resourceCreated.Name, resourceCreated.Count);
                _resources.Add(resource);
                break;
            }
            case ResourceRemoved resourceRemoved:
            {
                var resource = _resources.First(item => item.Id == resourceRemoved.ResourceId);
                _resources.Remove(resource);
                break;
            }
            case ConnectionAdded connectionAdded:
            {
                var parent = GetEntityById(_tasks, connectionAdded.MainTaskId);
                var child = GetEntityById(_tasks, connectionAdded.ChildTaskId);
                var connection = new TaskConnection(connectionAdded.ConnectionId, parent, child, connectionAdded.ConnectionType);
                parent.AddConnection(connection);
                child.AddConnection(connection);
                break;
            }
            case ConnectionRemoved connectionRemoved:
            {
                var parent = GetEntityById(_tasks, connectionRemoved.MainTaskId);
                var child = GetEntityById(_tasks, connectionRemoved.ChildTaskId);
                var connection = parent.GetConnectionByChild(child);
                parent.RemoveConnection(connection);
                child.RemoveConnection(connection);
                break;
            }
            case SubtaskAdded subtaskAdded:
            {
                var parent = GetEntityById(_tasks, subtaskAdded.MainTaskId);
                var child = GetEntityById(_tasks, subtaskAdded.ChildTaskId);
                var hierarchy = new TaskHierarchy(subtaskAdded.HierarchyId, parent, child);
                parent.AddHierarchy(hierarchy);
                child.AddHierarchy(hierarchy);
                break;
            }
            case SubtaskRemoved subtasRemoved:
            {
                var parent = GetEntityById(_tasks, subtasRemoved.MainTaskId);
                var child = GetEntityById(_tasks, subtasRemoved.ChildTaskId);
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

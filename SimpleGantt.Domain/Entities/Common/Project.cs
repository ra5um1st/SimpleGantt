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
using static SimpleGantt.Domain.Queries.CommonQueries;
using static SimpleGantt.Domain.Queries.ProjectQueries;

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
        if (Id != taskCreated.ProjectId) throw new DomainEventException(nameof(taskCreated));

        var task = new Task(taskCreated.TaskId, this, taskCreated.Name, taskCreated.StartDate, taskCreated.FinishDate, taskCreated.CompletionPercentage);
        _tasks.Add(task);
        AddDomainEvent(taskCreated);

        return task;
    }

    public void RemoveTask(TaskRemoved taskRemoved)
    {
        if (Id != taskRemoved.ProjectId) throw new DomainEventException(nameof(taskRemoved));

        Task task = GetEntityById(_tasks, taskRemoved.TaskId);
        _tasks.Remove(task);
        AddDomainEvent(taskRemoved);
    }

    public void ChangeTaskStartDate(TaskStartDateChanged startDateChanged)
    {
        Task task = GetEntityById(_tasks, startDateChanged.TaskId);

        if (task.StartDate == startDateChanged.NewStartDate)
        {
            return;
        }

        task.StartDate = startDateChanged.NewStartDate;
        AddDomainEvent(startDateChanged);
    }

    public void ChangeTaskFinishDate(TaskFinishDateChanged finishDateChanged)
    {
        Task task = GetEntityById(_tasks, finishDateChanged.TaskId);

        if (task.FinishDate == finishDateChanged.NewFinishDate)
        {
            return;
        }

        task.FinishDate = finishDateChanged.NewFinishDate;
        AddDomainEvent(finishDateChanged);
    }

    public void ChangeTaskCompletionPercentage(TaskCompletionPercentageChanged completionPercentageChanged)
    {
        // TODO: Add buisness rules for Completion Percentage change
        Task task = GetEntityById(_tasks, completionPercentageChanged.TaskId);

        if (task.CompletionPercentage == completionPercentageChanged.NewCompletionPercentage)
        {
            return;
        }

        task.CompletionPercentage = completionPercentageChanged.NewCompletionPercentage;
        AddDomainEvent(completionPercentageChanged);
    }

    public void ChangeTaskName(TaskNameChanged nameChanged)
    {
        Task task = GetEntityById(_tasks, nameChanged.EntityId);

        if (task.Name == nameChanged.NewName)
        {
            return;
        }

        task.Name = nameChanged.NewName;
        AddDomainEvent(nameChanged);
    }


    public Resource CreateResource(ResourceCreated resourceCreated)
    {
        if (Id != resourceCreated.ProjectId) throw new DomainEventException(nameof(resourceCreated));

        var resource = new Resource(resourceCreated.ResourceId, this, resourceCreated.Name, resourceCreated.Count);
        _resources.Add(resource);
        AddDomainEvent(resourceCreated);

        return resource;
    }

    public void RemoveResource(ResourceRemoved resourceRemoved)
    {
        if (Id != resourceRemoved.ProjectId) throw new DomainEventException(nameof(resourceRemoved));

        var resource = GetEntityById(_resources, resourceRemoved.ResourceId);
        _resources.Remove(resource);
        AddDomainEvent(new ResourceRemoved(Id, resource.Id));
    }

    public TaskResource AddResourceToTask(TaskResourceAdded taskResourceAdded)
    {
        Task task = GetEntityById(_tasks, taskResourceAdded.TaskId);
        Resource resource = GetEntityById(_resources, taskResourceAdded.ResourceId);
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
        if (amountChanged.Count == 0)
        {
            return;
        }

        Task task = GetEntityById(_tasks, amountChanged.TaskId);
        Resource resource = GetEntityById(_resources, amountChanged.TaskResourceId);
        TaskResource taskResource = task.Resources.First(item => item.Id == amountChanged.TaskResourceId);
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
        Task task = GetEntityById(_tasks, taskResourceRemoved.TaskId);
        Task resource = GetEntityById(_tasks, taskResourceRemoved.ResourceId);
        TaskResource taskResource = task.Resources.First(item => item.Id == taskResourceRemoved.TaskResourceId);
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
                if (child.StartDate >= parent.StartDate)
                {
                    return;
                }

                TimeSpan difference = parent.StartDate - child.StartDate;

                if (difference > TimeSpan.Zero)
                {
                    ChangeTaskStartDate(new TaskStartDateChanged(child.Id, parent.StartDate));
                    ChangeTaskFinishDate(new TaskFinishDateChanged(child.Id, child.FinishDate + difference));
                }

                break;
            }
            case nameof(ConnectionType.StartFinish):
            {
                if (child.FinishDate >= parent.StartDate)
                {
                    return;
                }

                TimeSpan difference = parent.StartDate - child.FinishDate;

                if (difference > TimeSpan.Zero)
                {
                    ChangeTaskStartDate(new TaskStartDateChanged(child.Id, child.StartDate + difference));
                    ChangeTaskFinishDate(new TaskFinishDateChanged(child.Id, parent.StartDate));
                }

                break;
            }
            case nameof(ConnectionType.FinishStart):
            {
                if (child.StartDate >= parent.FinishDate)
                {
                    return;
                }

                TimeSpan difference = parent.FinishDate - child.StartDate;

                if (difference > TimeSpan.Zero)
                {
                    ChangeTaskStartDate(new TaskStartDateChanged(child.Id, parent.FinishDate));
                    ChangeTaskFinishDate(new TaskFinishDateChanged(child.Id, child.FinishDate + difference));
                }

                break;
            }
            case nameof(ConnectionType.FinishFinish):
            {
                if (child.StartDate >= parent.FinishDate)
                {
                    return;
                }

                TimeSpan difference = parent.FinishDate - child.FinishDate;

                if (difference > TimeSpan.Zero)
                {
                    ChangeTaskStartDate(new TaskStartDateChanged(child.Id, child.StartDate + difference));
                    ChangeTaskFinishDate(new TaskFinishDateChanged(child.Id, parent.FinishDate));
                }

                break;
            }
            default:
                throw new DomainException($"Cannot apply connection to {parent.Name} and {child.Name} of type {connectionType.Name}");
        }
    }

    public TaskConnection AddConnection(ConnectionAdded connectionAdded)
    {
        Task parent = GetEntityById(_tasks, connectionAdded.MainTaskId);
        Task child = GetEntityById(_tasks, connectionAdded.ChildTaskId);

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
        Task parent = GetEntityById(_tasks, connectionRemoved.MainTaskId);
        Task child = GetEntityById(_tasks, connectionRemoved.ChildTaskId);

        if (!parent.HasConnectionWithChild(child))
        {
            throw new DomainNotExistException(nameof(child));
        }

        TaskConnection connection = parent.Connections.First(item => item.Parent.Id == Id && item.Child.Id == child.Id);
        parent.RemoveConnection(connection);
        child.RemoveConnection(connection);
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
                Task task = GetEntityById(_tasks, taskRemoved.TaskId);
                _tasks.Remove(task);
                break;
            }
            case TaskNameChanged nameChanged:
            {
                Task task = GetEntityById(_tasks, nameChanged.EntityId);
                task.Name = nameChanged.NewName;
                break;
            }
            case TaskStartDateChanged startDateChanged:
            {
                Task task = GetEntityById(_tasks, startDateChanged.TaskId);
                task.StartDate = startDateChanged.NewStartDate;
                break;
            }
            case TaskFinishDateChanged finishDateChanged:
            {
                Task task = GetEntityById(_tasks, finishDateChanged.TaskId);
                task.FinishDate = finishDateChanged.NewFinishDate;
                break;
            }
            case TaskCompletionPercentageChanged percentageChanged:
            {
                Task task = GetEntityById(_tasks, percentageChanged.TaskId);
                task.CompletionPercentage = percentageChanged.NewCompletionPercentage;
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
                Resource resource = _resources.First(item => item.Id == resourceRemoved.ResourceId);
                _resources.Remove(resource);
                break;
            }
            case ConnectionAdded connectionAdded:
            {
                Task parent = GetEntityById(_tasks, connectionAdded.MainTaskId);
                Task child = GetEntityById(_tasks, connectionAdded.ChildTaskId);
                var connection = new TaskConnection(connectionAdded.ConnectionId, parent, child, connectionAdded.ConnectionType);
                parent.AddConnection(connection);
                child.AddConnection(connection);
                break;
            }
            case ConnectionRemoved connectionRemoved:
            {
                Task parent = GetEntityById(_tasks, connectionRemoved.MainTaskId);
                Task child = GetEntityById(_tasks, connectionRemoved.ChildTaskId);
                TaskConnection connection = parent.GetConnectionByChild(child);
                parent.RemoveConnection(connection);
                child.RemoveConnection(connection);
                break;
            }
            case SubtaskAdded subtaskAdded:
            {
                Task parent = GetEntityById(_tasks, subtaskAdded.MainTaskId);
                Task child = GetEntityById(_tasks, subtaskAdded.ChildTaskId);
                var hierarchy = new TaskHierarchy(subtaskAdded.HierarchyId, parent, child);
                parent.AddHierarchy(hierarchy);
                child.AddHierarchy(hierarchy);
                break;
            }
            case SubtaskRemoved subtasRemoved:
            {
                Task parent = GetEntityById(_tasks, subtasRemoved.MainTaskId);
                Task child = GetEntityById(_tasks, subtasRemoved.ChildTaskId);
                TaskHierarchy hierarchy = parent.GetHierarchyByChild(child);
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
                base.Remove();
                break;
            }
            default:
                break;
        }
    }

    public override void Remove()
    {
        base.Remove();
        AddDomainEvent(new ProjectRemoved(Id));
    }

    public static Project RestoreFrom(IEnumerable<DomainEvent> events)
    {
        var project = new Project();

        foreach (DomainEvent @event in events)
        {
            project.When(@event);
            project.Version++;
        }

        return project;
    }

    #endregion
}

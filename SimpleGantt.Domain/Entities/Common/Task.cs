using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using SimpleGantt.Domain.Exceptions;
using SimpleGantt.Domain.ValueObjects;
using SimpleGantt.Domain.Interfaces;
using SimpleGantt.Domain.Events;
using SimpleGantt.Domain.Entities.Common;
using static SimpleGantt.Domain.Events.Common.TaskEvents;
using static SimpleGantt.Domain.Events.Common.DomainEvents;

namespace SimpleGantt.Domain.Entities;

public sealed class Task : AggregateRoot, INamable
{
    #region Fields

    private readonly HashSet<TaskConnection> _connections = new();
    private readonly HashSet<TaskHierarchy> _hierarchy = new();
    private readonly HashSet<TaskResource> _resources = new();

    #endregion

    #region Properties

    public EntityName Name { get; private set; }
    public DateTimeOffset StartDate { get; private set; }
    public DateTimeOffset FinishDate { get; private set; }
    public Percentage CompletionPercentage { get; private set; }
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

    #endregion

    #region Constructors

    private Task() : base(Guid.Empty)
    {
        Project = null!;
        Name = string.Empty;
        CompletionPercentage = 0;
    }

    internal Task(Guid id, Project project, EntityName name, DateTimeOffset startDate, DateTimeOffset finishData, Percentage percentage) : base(id)
    {
        Project = project ?? throw new ArgumentNullException(nameof(Project));
        Name = name;
        StartDate = startDate;
        FinishDate = finishData;
        CompletionPercentage = percentage;

        AddDomainEvent(new TaskCreated(Project.Id, Id, Name, StartDate, FinishDate, CompletionPercentage));
    }

    #endregion

    #region Methods

    public void ChangeStartDate(DateTimeOffset newDateTime)
    {
        if (newDateTime == StartDate) return;
        StartDate = newDateTime;
        AddDomainEvent(new StartDateChanged(Id, newDateTime));
    }

    public override void Remove()
    {
        if (Removed) throw new DomainException($"Task with id {Id} has already removed");
        Removed = true;
        AddDomainEvent(new TaskRemoved(Project.Id, Id));
    }

    public void ChangeFinishDate(DateTimeOffset newDateTime)
    {
        if (newDateTime == FinishDate) return;
        FinishDate = newDateTime;
        AddDomainEvent(new FinishDateChanged(Id, newDateTime));
    }

    public void ChangeCompletionPercentage(Percentage newPercentage)
    {
        // TODO: Add buisness rules for Completion Percentage change
        if (newPercentage == CompletionPercentage) return;
        CompletionPercentage = newPercentage;
        AddDomainEvent(new CompletionPercentageChanged(Id, newPercentage));
    }

    public void ChangeName(EntityName newName)
    {
        if (newName == Name) return;
        Name = newName;
        AddDomainEvent(new NameChanged(Id, newName));
    }

    public void AddConnection(TaskConnection connection)
    {
        if (HasConnectionWithChild(connection.Child))
        {
            throw new DomainExistsException(nameof(connection));
        }

        _connections.Add(connection);
    }

    public void RemoveConnection(TaskConnection connection)
    {
        if (!HasConnectionWithChild(connection.Child))
        {
            throw new DomainNotExistException(nameof(connection));
        }

        _connections.Remove(connection);
    }

    public void AddTaskResource(TaskResource taskResource)
    {
        if (_resources.Contains(taskResource))
        {
            throw new DomainExistsException(nameof(taskResource));
        }

        _resources.Add(taskResource);
    }

    public void RemoveTaskResource(TaskResource taskResource)
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

    public void AddHierarchy(TaskHierarchy hierarchy)
    {
        if (HasHierarchyWithChild(hierarchy.Child))
        {
            throw new DomainExistsException(nameof(hierarchy));
        }

        _hierarchy.Add(hierarchy);
    }

    public void RemoveHierarchy(TaskHierarchy hierarchy) 
    {
        if (!HasHierarchyWithChild(hierarchy.Child))
        {
            throw new DomainNotExistException(nameof(hierarchy));
        }

        _hierarchy.Remove(hierarchy); 
    }

    private bool HasHierarchyWithChild(Task child)
    {
        return _hierarchy.Any(item => item.Parent.Id == Id && item.Child.Id == child.Id);
    }

    public TaskHierarchy GetHierarchyByChild(Task child)
    {
        return _hierarchy.First(
            item => item.Parent.Id == Id && 
            item.Child.Id == child.Id);
    }

    public static Task RestoreFrom(IEnumerable<DomainEvent> events)
    {
        var task = new Task();

        foreach (var @event in events)
        {
            task.When(@event);
        }

        return task;
    }

    protected override void When(DomainEvent @event)
    {
        switch (@event)
        {
            case TaskCreated taskCreated:
            {
                Id = taskCreated.TaskId;
                Name = taskCreated.Name;
                StartDate = taskCreated.StartDate;
                FinishDate = taskCreated.FinishDate;
                CompletionPercentage = taskCreated.CompletionPercentage;
                break;
            }
            case TaskRemoved taskRemoved:
            {
                Removed = true;
                break;
            }
            case NameChanged nameChanged:
            {
                Name = nameChanged.NewEntityName;
                break;
            }
            case StartDateChanged startDateChanged:
            {
                StartDate = startDateChanged.NewStartDate;
                break;
            }
            case FinishDateChanged finishDateChanged:
            {
                FinishDate = finishDateChanged.NewFinishDate;
                break;
            }
            case CompletionPercentageChanged percentageChanged:
            {
                CompletionPercentage = percentageChanged.NewCompletionPercentage;
                break;
            }
            default:
                break;
        }
    }

    #endregion
}

using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using SimpleGantt.Domain.Exceptions;
using SimpleGantt.Domain.ValueObjects;
using SimpleGantt.Domain.Interfaces;
using SimpleGantt.Domain.Events;

using static SimpleGantt.Domain.Events.Common.TaskEvents;
using static SimpleGantt.Domain.Events.Common.DomainEvents;
using SimpleGantt.Domain.Entities.Common;

namespace SimpleGantt.Domain.Entities;

public sealed class Task : AggregateRoot, INamable
{
    #region Fields

    private readonly HashSet<TaskConnection> _taskConnections = new();
    private readonly HashSet<TaskHierarchy> _taskHierarchy = new();

    #endregion

    #region Properties

    public EntityName Name { get; private set; } = string.Empty;
    public DateTimeOffset StartDate { get; private set; }
    public DateTimeOffset FinishDate { get; private set; }
    public Percentage CompletionPercentage { get; private set; } = 0;
    public Project Project { get; private set; }
    public IReadOnlyCollection<TaskConnection> TaskConnections => _taskConnections;
    public IReadOnlyCollection<TaskHierarchy> TaskHierarchy => _taskHierarchy;

    [NotMapped]
    public bool HasChilds => _taskConnections.Any();

    [NotMapped]
    public bool HasConnections => _taskHierarchy.Any();

    [NotMapped]
    public bool IsMainTask => _taskHierarchy.Select(item => item.Child.Id != Id).Any();

    #endregion

    #region Constructors

    private Task()
    {
        Project = null!;
    }

    public Task(Project project, EntityName name, DateTimeOffset startDate, DateTimeOffset finishData, Percentage percentage)
    {
        Project = project;
        Name = name;
        StartDate = startDate;
        FinishDate = finishData;
        CompletionPercentage = percentage;

        AddDomainEvent(new TaskCreated(Id, project.Id, name, startDate, finishData, percentage));
    }

    #endregion

    #region Methods

    // TODO: Change dates with constraints

    public void ChangeStartDate(DateTimeOffset newDateTime)
    {
        // TODO: Add buisness rules Start Date change
        StartDate = newDateTime;
        AddDomainEvent(new StartDateChanged(Id, newDateTime));
    }

    public void ChangeFinishDate(DateTimeOffset newDateTime)
    {
        // TODO: Add buisness rules Finish Date change
        FinishDate = newDateTime;
        AddDomainEvent(new FinishDateChanged(Id, newDateTime));
    }

    public void ChangeCompletionPercentage(Percentage newPercentage)
    {
        // TODO: Add buisness rules for Completion Percentage change
        CompletionPercentage = newPercentage;
        AddDomainEvent(new CompletionPercentageChanged(Id, newPercentage));
    }

    public void ChangeName(EntityName newName)
    {
        Name = newName;
        AddDomainEvent(new NameChanged(Id, newName));
    }

    public void AddConnection(TaskConnection connection)
    {
        if (HasConnectionWithChild(connection.Child))
        {
            throw new DomainExistsException(this, connection.Child);
        }

        _taskConnections.Add(connection);
    }

    public void RemoveConnection(TaskConnection connection)
    {
        if (!HasConnectionWithChild(connection.Child))
        {
            throw new DomainNotExistException(this, connection.Child);
        }

        _taskConnections.Remove(connection);
    }

    public bool HasConnectionOfType(ConnectionType connectionType)
    {
        return _taskConnections
                    .Select(item => item.ConnectionType.Name)
                    .Any(item => item == connectionType.Name);
    }

    public bool HasConnectionWithChild(Task child)
    {
        return _taskConnections
                    .Select(item => item.Child.Id)
                    .Any(item => item == child.Id);
    }

    public bool HasConnectionWithChildOfType(Task child, ConnectionType connectionType)
    {
        return _taskConnections
                    .Select(item => new { item.Child.Id, ConnectionTypeName = item.ConnectionType.Name })
                    .Any(item => item.Id == child.Id && item.ConnectionTypeName == connectionType.Name);
    }

    public bool HasConnectionWithParentOfTYpe(Task parent, ConnectionType connectionType)
    {
        return _taskConnections
                    .Select(item => new { item.Parent.Id, ConnectionTypeName = item.ConnectionType.Name })
                    .Any(item => item.Id == parent.Id && item.ConnectionTypeName == connectionType.Name);
    }

    public TaskConnection GetConnectionByChild(Task child)
    {
        return TaskConnections.First(
            item => item.Parent.Id == Id &&
            item.Child.Id == child.Id);
    }

    public void AddHierarchy(TaskHierarchy hierarchy)
    {
        if (HasHierarchyWithChild(hierarchy.Child))
        {
            throw new DomainExistsException();
        }

        _taskHierarchy.Add(hierarchy);
    }

    public void RemoveHierarchy(TaskHierarchy hierarchy) 
    {
        if (!HasHierarchyWithChild(hierarchy.Child))
        {
            throw new DomainExistsException();
        }

        _taskHierarchy.Remove(hierarchy); 
    }

    private bool HasHierarchyWithChild(Task child)
    {
        return _taskHierarchy.Any(item => item.Parent.Id == Id && item.Child.Id == child.Id);
    }

    public TaskHierarchy GetHierarchyByChild(Task child)
    {
        return _taskHierarchy.First(
            item => item.Parent.Id == Id && 
            item.Child.Id == child.Id);
    }

    public static Task RestoreFromEvents(IEnumerable<DomainEvent> events)
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
                Name = taskCreated.Name;
                StartDate = taskCreated.StartDate;
                FinishDate = taskCreated.FinishDate;
                CompletionPercentage = taskCreated.CompletionPercentage;
                break;
            }
            case TaskRemoved taskRemoved:
            {
                IsRemoved = true;
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
            default:
                break;
        }
    }

    #endregion
}

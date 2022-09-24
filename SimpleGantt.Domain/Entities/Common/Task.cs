using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using SimpleGantt.Domain.Entities.Abstractions;
using SimpleGantt.Domain.Entities.DomainTypes;
using SimpleGantt.Domain.Events.Common;
using SimpleGantt.Domain.Events.Common.TaskEvents;
using SimpleGantt.Domain.Exceptions.Common;
using SimpleGantt.Domain.ValueObjects;

namespace SimpleGantt.Domain.Entities.Common;

public class Task : NamedEntity
{
    #region Fields

    private HashSet<TaskConnection> _taskConnections = new HashSet<TaskConnection>();
    private HashSet<TaskConnection> _taskHierarchy = new HashSet<TaskConnection>();

    #endregion

    #region Properties

    public DateTimeOffset StartDate { get; private set; }

    public DateTimeOffset FinishDate { get; private set; }

    public Percentage CompletionPercentage { get; private set; }

    public IReadOnlyCollection<TaskConnection> TaskConnections => _taskConnections;

    public IReadOnlyCollection<TaskConnection> TaskHierarchy => _taskHierarchy;

    [NotMapped]
    public bool HasChilds => _taskConnections.Any();

    [NotMapped]
    public bool HasConnections => _taskHierarchy.Any();

    [NotMapped]
    public bool IsMainTask => _taskHierarchy.Select(item => item.Child.Id != Id).Any();

    #endregion

    #region Constructors

    private Task() : base(string.Empty)
    {
        CompletionPercentage = 0;
    }

    public Task(string name, DateTimeOffset startDate, DateTimeOffset finishDate, Percentage percentage) : base(name)
    {
        StartDate = startDate;
        FinishDate = finishDate;
        CompletionPercentage = percentage ?? 0;

        AddDomainEvent(new TaskCreatedEvent(this));
    }

    #endregion

    #region Methods

    // TODO: Change dates with constraints

    public void ChangeStartDate(DateTimeOffset newDateTime)
    {
        // TODO: Add buisness rules Start Date change

        StartDate = newDateTime;
        AddDomainEvent(new EntityModifiedEvent<Task, DateTimeOffset>(this, nameof(StartDate), StartDate, newDateTime));
    }

    public void ChangeFinishDate(DateTimeOffset newDateTime)
    {
        // TODO: Add buisness rules Finish Date change

        FinishDate = newDateTime;
        AddDomainEvent(new EntityModifiedEvent<Task, DateTimeOffset>(this, nameof(FinishDate), FinishDate, newDateTime));
    }

    public void ChangeCompletionPercentage(Percentage newPercentage)
    {
        // TODO: Add buisness rules for Completion Percentage change

        CompletionPercentage = newPercentage;
        AddDomainEvent(new EntityModifiedEvent<Task, Percentage>(this, nameof(CompletionPercentage), CompletionPercentage, newPercentage));
    }

    public void ChangeName(string newName)
    {
        Name = newName;
        AddDomainEvent(new EntityModifiedEvent<Task, string>(this, nameof(Name), Name, newName));
    }

    public void AddTaskConnection(Task child, ConnectionType connectionType)
    {
        if (HasTaskConnectionWithChild(child))
        {
            throw new AlreadyExistsException(this, child);
        }

        _taskConnections.Add(new TaskConnection(this, child, connectionType));
    }

    public void RemoveTaskConnection(Task child)
    {
        if (!HasTaskConnectionWithChild(child))
        {
            throw new NotExistException(this, child);
        }

        var taskConnection = _taskConnections.First(item => item.Parent.Id == Id && item.Child.Id == child.Id);
        _taskConnections.Remove(taskConnection);
    }

    public bool HasTaskConnectionOfType(ConnectionType connectionType)
    {
        return _taskConnections
                    .Select(item => item.ConnectionType.Name)
                    .Any(item => item == connectionType.Name);
    }

    public bool HasTaskConnectionWithChild(Task child)
    {
        return _taskConnections
                    .Select(item => item.Child.Id)
                    .Any(item => item == child.Id);
    }

    public bool HasTaskConnectionWithChildOfType(Task child, ConnectionType connectionType)
    {
        return _taskConnections
                    .Select(item => new { item.Child.Id, ConnectionTypeName =  item.ConnectionType.Name })
                    .Any(item => item.Id == child.Id && item.ConnectionTypeName == connectionType.Name);
    }

    public bool HasTaskConnectionWithParentOfTYpe(Task parent, ConnectionType connectionType)
    {
        return _taskConnections
                    .Select(item => new { item.Parent.Id, ConnectionTypeName = item.ConnectionType.Name })
                    .Any(item => item.Id == parent.Id && item.ConnectionTypeName == connectionType.Name);
    }


    public void AddTaskHierarchy()
    {

    }

    public void RemoveTaskHierarchy()
    {

    }

    #endregion
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json;
using SimpleGantt.Domain.Entities.Abstractions;
using SimpleGantt.Domain.Events.Abstractions;
using SimpleGantt.Domain.Events.Common;
using SimpleGantt.Domain.Events.Common.TaskEvents;
using SimpleGantt.Domain.ValueObjects;

namespace SimpleGantt.Domain.Entities.Common;

public class Task : NamedEntity
{
    #region Properties

    public DateTimeOffset StartDate { get; private set; }
    public DateTimeOffset FinishDate { get; private set; }
    public Percentage CompletionPercentage { get; private set; } = 0;

    private HashSet<TaskConnection> _taskConnections = new HashSet<TaskConnection>();
    public IReadOnlyCollection<TaskConnection> TaskConnections => _taskConnections;

    private HashSet<TaskConnection> _taskHierarchy = new HashSet<TaskConnection>();
    public IReadOnlyCollection<TaskConnection> TaskHierarchy => _taskHierarchy;

    [NotMapped]
    public bool HasChilds => _taskConnections.Any();

    [NotMapped]
    public bool HasConnections => _taskHierarchy.Any();

    [NotMapped]
    public bool IsMainTask => !_taskHierarchy.Select(item => item.Child.Id).Contains(Id);

    #endregion

    #region Constructors

    private Task() : base(string.Empty)
    {

    }

    public Task(
        string name, 
        DateTimeOffset startDate, 
        DateTimeOffset finishDate, 
        Percentage percentage) : base(name)
    {
        StartDate = startDate;
        FinishDate = finishDate;
        CompletionPercentage = percentage ?? throw new ArgumentNullException(nameof(percentage));

        AddDomainEvent(new TaskCreatedEvent() { Name = name, ComplitionPercentage = percentage, StartDate = startDate, FinishDate = finishDate });
    }

    #endregion

    #region Methods

    public bool ChangeStartDate(DateTimeOffset newDateTime)
    {
        if(HasChilds || IsMainTask || HasConnections)
        {
            return false;
        }

        StartDate = newDateTime;
        _domainEvents.Add(new PropertyModifiedEvent<Task, DateTimeOffset>(this, nameof(StartDate), StartDate, newDateTime));

        return true;
    }

    public Task RestoreFromEvents(IEnumerable<DomainEvent> events)
    {
        var task = new Task();

        foreach (var @event in events)
        {
            switch (@event)
            {
                case TaskCreatedEvent taskCreatedEvent:
                {
                    task = new Task(
                        taskCreatedEvent.Name, 
                        taskCreatedEvent.StartDate, 
                        taskCreatedEvent.FinishDate, 
                        taskCreatedEvent.ComplitionPercentage);
                    break;
                }
                case PropertyModifiedEvent<Task, DateTimeOffset> startDateModified:
                {
                    task.StartDate = startDateModified.NewValue;
                    break;
                }
                default:
                    break;
            }
        }

        return task;
    }

    // TODO: Change dates with constraints

    #endregion
}

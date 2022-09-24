using System;
using System.Collections.Generic;
using System.Text.Json;
using SimpleGantt.Domain.Entities.Common;
using SimpleGantt.Domain.Events.Abstractions;
using SimpleGantt.Domain.Events.Common;
using SimpleGantt.Domain.Events.Common.TaskEvents;
using SimpleGantt.Domain.Interfaces;

namespace SimpleGantt.Domain.Factories;

public class TaskFactory : IRestoreFromEvents<Task>
{
    public Task RestoreFromEvents(IEnumerable<DomainEvent> events)        
    {
        Task task = new Task("RestoreFromEvents", DateTimeOffset.Now, DateTimeOffset.Now, 0);

        foreach (var @event in events)
        {
            switch (@event)
            {
                case TaskCreatedEvent taskCreatedEvent:
                {
                    task = new Task(taskCreatedEvent.Name, taskCreatedEvent.StartDate, taskCreatedEvent.FinishDate, taskCreatedEvent.CompletionPercentage);
                    break;
                }
                case EntityModifiedEvent<Task, DateTimeOffset> startDateModified:
                {
                    task.ChangeStartDate(startDateModified.NewValue);
                    break;
                }
                //case EntityModifiedEvent<Task, DateTimeOffset> finishDateModified:
                //{
                //    task.ChangeFinishDate(finishDateModified.NewValue);
                //    break;
                //}
                default:
                    break;
            }
        }

        return task;
    }
}

using System;
using SimpleGantt.Domain.Entities.Abstractions;

namespace SimpleGantt.Domain.Entities.EventSourcing;

public class TaskEventLog : EventLog
{
    public TaskEventLog(string eventName, string version, string propertyChanged, string propertyChangedType, string oldValue, string newValue) 
        : base(eventName, version, propertyChanged, propertyChangedType, oldValue, newValue)
    {
    }
}

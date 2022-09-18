using System;

namespace SimpleGantt.Domain.Entities.Abstractions;

public class EventLog : Entity
{
    public DateTimeOffset EventTime { get; set; }
    public string EventName { get; set; }
    public string Version { get; set; }
    public string PropertyChanged { get; set; }
    public string PropertyChangedType { get; set; }
    public string OldValue { get; set; }
    public string NewValue { get; set; }

    public EventLog(string eventName, string version, string propertyChanged, string propertyChangedType, string oldValue, string newValue)
    {
        EventTime = DateTimeOffset.Now;
        EventName = eventName;
        Version = version;
        PropertyChanged = propertyChanged;
        PropertyChangedType = propertyChangedType;
        OldValue = oldValue;
        NewValue = newValue;
    }
}

namespace SimpleGantt.Domain.Events.Abstractions;

public abstract class DomainModifiedEvent : DomainEvent
{
    public string PropertyName { get; }
    public object OldValue { get; }
    public object NewValue { get; }

    public DomainModifiedEvent(string propertyName, object oldValue, object newValue) : base()
    {
        PropertyName = propertyName;
        OldValue = oldValue;
        NewValue = newValue;
    }
}

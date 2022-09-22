namespace SimpleGantt.Domain.Events.Abstractions;

public abstract class DomainModifiedEvent<TProperty> : DomainEvent
{
    public string PropertyName { get; }
    public TProperty OldValue { get; }
    public TProperty NewValue { get; }

    public DomainModifiedEvent(string propertyName, TProperty oldValue, TProperty newValue) : base()
    {
        PropertyName = propertyName;
        OldValue = oldValue;
        NewValue = newValue;
    }
}

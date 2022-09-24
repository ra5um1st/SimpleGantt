using SimpleGantt.Domain.Entities.Abstractions;
using SimpleGantt.Domain.Events.Abstractions;

namespace SimpleGantt.Domain.Events.Common;

public record EntityModifiedEvent<TEntity, TProperty> : DomainEvent where TEntity : Entity
{
    public string PropertyName { get; }
    public string PropertyTypeName { get; }
    public TProperty OldValue { get; }
    public TProperty NewValue { get; }

    public EntityModifiedEvent(TEntity entity, string propertyName, TProperty oldValue, TProperty newValue) : base(entity)
    {
        PropertyName = propertyName;
        PropertyTypeName = nameof(TProperty);
        OldValue = oldValue;
        NewValue = newValue;
    }
}

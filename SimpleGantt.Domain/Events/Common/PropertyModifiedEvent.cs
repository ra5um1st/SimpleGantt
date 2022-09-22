using SimpleGantt.Domain.Entities.Abstractions;
using SimpleGantt.Domain.Events.Abstractions;

namespace SimpleGantt.Domain.Events.Common;

public class PropertyModifiedEvent<TEntity, TProperty> : DomainModifiedEvent<TProperty> where TEntity : Entity
{
    public TEntity ModifiedEntity { get; }

    public PropertyModifiedEvent(TEntity modifiedEntity, string propertyName, TProperty oldValue, TProperty newValue) 
        : base(propertyName, oldValue, newValue)
    {
        ModifiedEntity = modifiedEntity;
    }
}

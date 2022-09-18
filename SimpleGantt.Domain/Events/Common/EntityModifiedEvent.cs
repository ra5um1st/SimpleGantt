using SimpleGantt.Domain.Entities.Abstractions;
using SimpleGantt.Domain.Events.Abstractions;

namespace SimpleGantt.Domain.Events.Common;

public class EntityModifiedEvent : DomainModifiedEvent
{
    public Entity ModifiedEntity { get; }

    public EntityModifiedEvent(Entity modifiedEntity, string propertyName, object oldValue, object newValue) 
        : base(propertyName, oldValue, newValue)
    {
        ModifiedEntity = modifiedEntity;
    }
}

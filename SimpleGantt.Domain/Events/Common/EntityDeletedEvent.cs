using SimpleGantt.Domain.Entities.Abstractions;
using SimpleGantt.Domain.Events.Abstractions;

namespace SimpleGantt.Domain.Events.Common;

public class EntityDeletedEvent : DomainEvent
{
    public Entity DeletedEntity { get; }

    public EntityDeletedEvent(Entity deletedEntity) : base()
    {
        DeletedEntity = deletedEntity;
    }
}

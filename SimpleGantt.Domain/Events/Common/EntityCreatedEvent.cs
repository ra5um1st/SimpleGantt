using SimpleGantt.Domain.Entities.Abstractions;
using SimpleGantt.Domain.Events.Abstractions;

namespace SimpleGantt.Domain.Events.Common;

public class EntityCreatedEvent : DomainEvent
{
    public Entity CreatedEntity { get; set; }

    public EntityCreatedEvent(Entity createdEntity) : base()
    {
        CreatedEntity = createdEntity;
    }
}

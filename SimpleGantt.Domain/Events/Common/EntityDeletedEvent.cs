using SimpleGantt.Domain.Entities.Abstractions;
using SimpleGantt.Domain.Events.Abstractions;

namespace SimpleGantt.Domain.Events.Common;

public record EntityDeletedEvent<TEntity> : DomainEvent where TEntity : Entity
{
    public EntityDeletedEvent(TEntity deletedEntity) : base(deletedEntity)
    {
    }
}

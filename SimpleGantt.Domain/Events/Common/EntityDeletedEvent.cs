using SimpleGantt.Domain.Entities.Abstractions;
using SimpleGantt.Domain.Events.Abstractions;

namespace SimpleGantt.Domain.Events.Common;

public class EntityDeletedEvent<TEntity> : DomainEvent where TEntity : Entity
{
    public TEntity DeletedEntity { get; }

    public EntityDeletedEvent(TEntity deletedEntity) : base()
    {
        DeletedEntity = deletedEntity;
    }
}

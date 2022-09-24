using SimpleGantt.Domain.Entities.Abstractions;

namespace SimpleGantt.Domain.Events.Abstractions;

public abstract record DomainCreatedEvent<TEntity> : DomainEvent where TEntity : Entity
{

    public DomainCreatedEvent(TEntity createdEntity) : base(createdEntity)
    {

    }
}

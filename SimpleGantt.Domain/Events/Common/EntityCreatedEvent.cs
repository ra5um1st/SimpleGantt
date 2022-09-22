using System.Text.Json;
using SimpleGantt.Domain.Entities.Abstractions;
using SimpleGantt.Domain.Events.Abstractions;

namespace SimpleGantt.Domain.Events.Common;

public class EntityCreatedEvent<TEntity> : DomainEvent where TEntity : Entity
{
    public TEntity CreatedEntity { get; }
    public string Data { get; }

    public EntityCreatedEvent(TEntity createdEntity) : base()
    {
        CreatedEntity = createdEntity;
        Data = JsonSerializer.Serialize(createdEntity);
    }
}

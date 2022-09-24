using SimpleGantt.Domain.ValueObjects;

namespace SimpleGantt.Domain.Entities.Abstractions;

public abstract class NamedEntity : Entity
{
    public EntityName Name { get; protected set; }

    public NamedEntity(EntityName name)
    {
        Name = name;
    }
}

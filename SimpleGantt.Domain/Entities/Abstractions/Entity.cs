using System;
using SimpleGantt.Domain.Interfaces;

namespace SimpleGantt.Domain.Entities;

public abstract class Entity : IEntity<Guid>
{
    public Guid Id { get; protected set; }

    public Entity(Guid id)
    {
        Id = id;
    }
}

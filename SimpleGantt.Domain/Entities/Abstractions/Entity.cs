using System;
using SimpleGantt.Domain.Interfaces;

namespace SimpleGantt.Domain.Entities;

public abstract class Entity : IEntity
{
    public Guid Id { get; protected set; }
}

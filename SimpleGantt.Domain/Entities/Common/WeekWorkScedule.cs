using System;
using SimpleGantt.Domain.Interfaces;
using SimpleGantt.Domain.ValueObjects;

namespace SimpleGantt.Domain.Entities;

public class WorkWeekScedule : Entity, ITrackable, INamable
{
    public EntityName Name { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset UpdatedAt { get; private set; }

    public WorkWeekScedule(EntityName name, DateTimeOffset createdAt, DateTimeOffset updatedAt)
    {
        Name = name;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }
}

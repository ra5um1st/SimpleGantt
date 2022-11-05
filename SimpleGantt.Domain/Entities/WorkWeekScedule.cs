using System;
using System.Collections.Generic;
using SimpleGantt.Domain.Interfaces;
using SimpleGantt.Domain.ValueObjects;

namespace SimpleGantt.Domain.Entities;

public sealed class WorkWeekScedule : Entity, ITrackable, INamable
{
    private readonly HashSet<WorkDay> _workDays = new();
    private readonly HashSet<DayOfWeekType> _dayOffs = new();

    public EntityName Name { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset UpdatedAt { get; private set; }
    public IReadOnlyCollection<WorkDay> WorkDays => _workDays;
    public IReadOnlyCollection<DayOfWeekType> DayOffs => _dayOffs;

    public WorkWeekScedule(Guid id, EntityName name, DateTimeOffset createdAt) : base(id)
    {
        Name = name;
        CreatedAt = createdAt;
        UpdatedAt = createdAt;
    }
}

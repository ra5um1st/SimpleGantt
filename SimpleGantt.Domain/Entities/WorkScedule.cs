using System;
using SimpleGantt.Domain.Interfaces;
using SimpleGantt.Domain.ValueObjects;

namespace SimpleGantt.Domain.Entities;

public class WorkScedule : Entity, INamable
{
    public EntityName Name { get; private set; }
    public WorkWeekScedule WorkWeekScedule { get; private set; }

    public WorkScedule(Guid id, EntityName name, WorkWeekScedule workWeekScedule) : base(id)
    {
        Name = name;
        WorkWeekScedule = workWeekScedule;
    }
}

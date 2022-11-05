using System;

namespace SimpleGantt.Domain.Entities;

public sealed class WorkDay : Entity
{
    public TimeOnly StartWorkTime { get; private set; }
    public TimeOnly FinishWorkTime { get; private set; }
    public WorkWeekScedule WorkWeekScedule { get; private set; }

    public WorkDay(Guid id, WorkWeekScedule weekWorkScedule, TimeOnly startWorkTime, TimeOnly finishWorkTime) : base(id)
    {
        WorkWeekScedule = weekWorkScedule;
        StartWorkTime = startWorkTime;
        FinishWorkTime = finishWorkTime;
    }
}

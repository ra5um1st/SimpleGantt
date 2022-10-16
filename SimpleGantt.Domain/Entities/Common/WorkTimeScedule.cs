using System;

namespace SimpleGantt.Domain.Entities;

public class WorkTimeScedule : Entity
{
    public TimeOnly StartWorkTime { get; private set; }
    public TimeOnly FinishWorkTime { get; private set; }

    public WorkTimeScedule(TimeOnly startWorkTime, TimeOnly finishWorkTime)
    {
        StartWorkTime = startWorkTime;
        FinishWorkTime = finishWorkTime;
    }
}
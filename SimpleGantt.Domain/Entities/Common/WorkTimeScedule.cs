using System;
using SimpleGantt.Domain.Entities.Abstractions;

namespace SimpleGantt.Domain.Entities.Common;

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
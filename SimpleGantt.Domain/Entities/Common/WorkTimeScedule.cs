using System;
using SimpleGantt.Domain.Entities.Base;

namespace SimpleGantt.Domain.Entities.Common;

public class WorkTimeScedule : Entity
{
    public TimeOnly StartWorkTime { get; set; }
    public TimeOnly FinishWorkTime { get; set; }

    public WorkTimeScedule(TimeOnly startWorkTime, TimeOnly finishWorkTime)
    {
        StartWorkTime = startWorkTime;
        FinishWorkTime = finishWorkTime;
    }
}
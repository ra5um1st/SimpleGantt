using SimpleGantt.Domain.Entities.Abstractions;

namespace SimpleGantt.Domain.Entities.Common;

public class WorkScedule : TrackedEntity
{
    public WorkTimeScedule WorkTimeScedule { get; private set; }
    public WorkWeekScedule WorkWeekScedule { get; private set; }

    public WorkScedule(string name, WorkTimeScedule workTimeScedule, WorkWeekScedule workWeekScedule) : base(name)
    {
        WorkTimeScedule = workTimeScedule;
        WorkWeekScedule = workWeekScedule;
    }
}

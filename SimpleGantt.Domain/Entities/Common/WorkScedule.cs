using SimpleGantt.Domain.Interfaces;
using SimpleGantt.Domain.ValueObjects;

namespace SimpleGantt.Domain.Entities;

public class WorkScedule : Entity, INamable
{
    public EntityName Name { get; private set; }
    public WorkTimeScedule WorkTimeScedule { get; private set; }
    public WorkWeekScedule WorkWeekScedule { get; private set; }

    public WorkScedule(EntityName name, WorkTimeScedule workTimeScedule, WorkWeekScedule workWeekScedule)
    {
        Name = name;
        WorkTimeScedule = workTimeScedule;
        WorkWeekScedule = workWeekScedule;
    }
}

using SimpleGantt.Domain.Entities.Abstractions;
using SimpleGantt.Domain.Entities.DomainTypes;

namespace SimpleGantt.Domain.Entities.Common;
public class WeekWorkSceduleDay : Entity
{
    public WorkWeekScedule WeekWorkScedule { get; private set; }
    public SceduleDayType SceduleDayType { get; private set; }

    public WeekWorkSceduleDay(WorkWeekScedule weekWorkScedule, SceduleDayType sceduleDayType)
    {
        WeekWorkScedule = weekWorkScedule;
        SceduleDayType = sceduleDayType;
    }
}

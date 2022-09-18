using SimpleGantt.Domain.Entities.Base;
using SimpleGantt.Domain.Entities.DomainTypes;

namespace SimpleGantt.Domain.Entities.Common;
public class WeekWorkSceduleDay : Entity
{
    public WorkWeekScedule WeekWorkScedule { get; set; }
    public SceduleDayType SceduleDayType { get; set; }

    public WeekWorkSceduleDay(WorkWeekScedule weekWorkScedule, SceduleDayType sceduleDayType)
    {
        WeekWorkScedule = weekWorkScedule;
        SceduleDayType = sceduleDayType;
    }
}

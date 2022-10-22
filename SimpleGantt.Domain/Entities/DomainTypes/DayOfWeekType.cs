using SimpleGantt.Domain.ValueObjects;

namespace SimpleGantt.Domain.Entities;

public record DayOfWeekType(EntityName Name) : DomainType(Name)
{
    public static DayOfWeekType Monday { get; } = new(1, nameof(Monday));
    public static DayOfWeekType Tuesday { get; } = new(2, nameof(Tuesday));
    public static DayOfWeekType Wednesday { get; } = new(3, nameof(Wednesday));
    public static DayOfWeekType Thursday { get; } = new(4, nameof(Thursday));
    public static DayOfWeekType Friday { get; } = new(5, nameof(Friday));
    public static DayOfWeekType Saturday { get; } = new(6, nameof(Saturday));
    public static DayOfWeekType Sunday { get; } = new(7, nameof(Sunday));

    private DayOfWeekType(long id, EntityName name) : this(name)
    {
        Id = id;
    }
}

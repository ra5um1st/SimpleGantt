using SimpleGantt.Domain.ValueObjects;

namespace SimpleGantt.Domain.Entities;

public record UnitOfTimeType(EntityName Name) : DomainType(Name)
{
    public static readonly UnitOfTimeType Hour  = new(1, nameof(Hour));
    public static readonly UnitOfTimeType Day   = new(2, nameof(Day));
    public static readonly UnitOfTimeType Week  = new(3, nameof(Week));
    public static readonly UnitOfTimeType Month = new(4, nameof(Month));
    public static readonly UnitOfTimeType Year  = new(5, nameof(Year));

    private UnitOfTimeType(long id, EntityName name) : this(name)
    {
        Id = id;
    }
}
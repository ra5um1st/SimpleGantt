using SimpleGantt.Domain.ValueObjects;

namespace SimpleGantt.Domain.Entities;

public record SalaryType(EntityName Name, CurrencyType CurrencyType, UnitOfTimeType UnitOfTimeType) : DomainType(Name)
{
    public static readonly SalaryType RubPerMonth = new(1, nameof(RubPerMonth), CurrencyType.RUB, UnitOfTimeType.Month);

    private SalaryType(long id, EntityName name, CurrencyType currencyType, UnitOfTimeType unitOfTimeType)
        : this(name, currencyType, unitOfTimeType)
    {
        Id = id;
    }
}
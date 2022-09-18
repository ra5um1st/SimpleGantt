using SimpleGantt.Domain.Entities.Abstractions;

namespace SimpleGantt.Domain.Entities.DomainTypes;

public record SalaryType : DomainType
{
    public CurrencyType CurrencyType { get; init; }
    public UnitTimeType UnitTimeType { get; init; }

    public SalaryType(long Id, CurrencyType currencyType, UnitTimeType unitTimeType) : base(Id)
    {
        CurrencyType = currencyType;
        UnitTimeType = unitTimeType;
    }
}
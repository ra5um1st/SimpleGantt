using System;
using SimpleGantt.Domain.ValueObjects;

namespace SimpleGantt.Domain.Entities;

public record SalaryType : DomainType
{
    public CurrencyType CurrencyType { get; }
    public UnitOfTimeType UnitTimeType { get; }

    public SalaryType(Guid id, EntityName name, CurrencyType currencyType, UnitOfTimeType unitTimeType) : base(id, name)
    {
        Name = name;
        CurrencyType = currencyType;
        UnitTimeType = unitTimeType;
    }
}
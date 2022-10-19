using SimpleGantt.Domain.ValueObjects;

namespace SimpleGantt.Domain.Entities;

public record SalaryType(EntityName Name, CurrencyType CurrencyType, UnitOfTimeType UnitOfTimeType) : DomainType(Name);
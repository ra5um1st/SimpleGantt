using SimpleGantt.Domain.ValueObjects;

namespace SimpleGantt.Domain.Entities;

public record CurrencyType(EntityName Name, CurrencyAbbreviation Abbreviation) : DomainType(Name);

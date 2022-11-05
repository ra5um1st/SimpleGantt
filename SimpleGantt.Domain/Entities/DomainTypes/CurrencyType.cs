using SimpleGantt.Domain.ValueObjects;

namespace SimpleGantt.Domain.Entities;

public record CurrencyType(EntityName Name, CurrencyAbbreviation Abbreviation) : DomainType(Name)
{
    public static readonly CurrencyType RUB = new CurrencyType(1, "Russian Ruble", "RUB");

    private CurrencyType(long id, EntityName name, CurrencyAbbreviation abbreviation) : this(name, abbreviation)
    {
        Id = id;
    }
}

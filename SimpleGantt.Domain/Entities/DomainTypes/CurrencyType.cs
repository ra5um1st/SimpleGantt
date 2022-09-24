using SimpleGantt.Domain.Entities.Abstractions;
using SimpleGantt.Domain.ValueObjects;

namespace SimpleGantt.Domain.Entities.DomainTypes;

public record CurrencyType : NamedDomainType
{
    public CurrencyAbbreviation Abbreviation { get; init; }

    public CurrencyType(long Id, EntityName name, CurrencyAbbreviation abbreviation) : base(Id, name)
    {
        Abbreviation = abbreviation;
    }
}

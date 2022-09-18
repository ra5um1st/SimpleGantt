using SimpleGantt.Domain.Entities.Base;

namespace SimpleGantt.Domain.Entities.DomainTypes;

public record CurrencyType : NamedDomainType
{
    public string Abbreviation { get; init; }

    public CurrencyType(long Id, string name, string abbreviation) : base(Id, name)
    {
        Abbreviation = abbreviation;
    }
}

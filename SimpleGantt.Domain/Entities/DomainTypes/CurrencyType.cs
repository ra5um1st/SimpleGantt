using System;
using SimpleGantt.Domain.ValueObjects;

namespace SimpleGantt.Domain.Entities;

public record CurrencyType : DomainType
{
    public CurrencyAbbreviation Abbreviation { get; init; }

    public CurrencyType(Guid id, EntityName name, CurrencyAbbreviation abbreviation) : base(id, name)
    {
        Abbreviation = abbreviation;
    }
}

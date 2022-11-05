using System;
using System.Data.SqlTypes;
using SimpleGantt.Domain.Entities;
using SimpleGantt.Domain.ValueObjects;

namespace SimpleGantt.Domain.Entities;

public class MaterialResource : Resource
{
    public SqlMoney Cost { get; private set; }
    public CurrencyType CurrencyType { get; private set; }

    public MaterialResource(Guid id, Project project, EntityName name, uint count, SqlMoney cost, CurrencyType currencyType) : base(id, project, name, count)
    {
        Cost = cost;
        CurrencyType = currencyType;
    }
}

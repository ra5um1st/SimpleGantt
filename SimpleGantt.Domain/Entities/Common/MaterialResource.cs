using System.Data.SqlTypes;
using SimpleGantt.Domain.ValueObjects;

namespace SimpleGantt.Domain.Entities;

public class MaterialResource : Resource
{
    public SqlMoney Cost { get; private set; }
    public CurrencyType CurrencyType { get; private set; }

    public MaterialResource(EntityName name, uint count, SqlMoney cost, CurrencyType currencyType) : base(name, count)
    {
        Cost = cost;
        CurrencyType = currencyType;
    }
}

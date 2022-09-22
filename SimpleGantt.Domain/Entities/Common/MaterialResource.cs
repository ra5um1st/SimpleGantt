using System.Data.SqlTypes;
using SimpleGantt.Domain.Entities.DomainTypes;

namespace SimpleGantt.Domain.Entities.Common;

public class MaterialResource : Resource
{
    public SqlMoney Cost { get; private set; }
    public CurrencyType CurrencyType { get; private set; }

    public MaterialResource(string name, uint count, SqlMoney cost, CurrencyType currencyType) : base(name, count)
    {
        Cost = cost;
        CurrencyType = currencyType;
    }
}

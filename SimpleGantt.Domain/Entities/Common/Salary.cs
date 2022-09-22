using System.Data.SqlTypes;
using SimpleGantt.Domain.Entities.Abstractions;
using SimpleGantt.Domain.Entities.DomainTypes;

namespace SimpleGantt.Domain.Entities.Common;

public class Salary : Entity
{
    public SqlMoney Value { get; private set; }
    public SalaryType SalaryType { get; private set; }

    public Salary(SqlMoney value, SalaryType salaryType)
    {
        Value = value;
        SalaryType = salaryType;
    }
}

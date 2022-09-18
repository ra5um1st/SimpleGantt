using System.Data.SqlTypes;
using SimpleGantt.Domain.Entities.Base;
using SimpleGantt.Domain.Entities.DomainTypes;

namespace SimpleGantt.Domain.Entities.Common;

public class Salary : Entity
{
    public SqlMoney Value { get; set; }
    public SalaryType SalaryType { get; set; }

    public Salary(SqlMoney value, SalaryType salaryType)
    {
        Value = value;
        SalaryType = salaryType;
    }
}

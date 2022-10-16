using System.Data.SqlTypes;

namespace SimpleGantt.Domain.Entities;

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

using System;
using System.Data.SqlTypes;

namespace SimpleGantt.Domain.Entities;

public class Salary : Entity
{
    public SqlMoney Value { get; private set; }
    public SalaryType SalaryType { get; private set; }

    public Salary(Guid id, SqlMoney value, SalaryType salaryType) : base(id)
    {
        Value = value;
        SalaryType = salaryType;
    }
}

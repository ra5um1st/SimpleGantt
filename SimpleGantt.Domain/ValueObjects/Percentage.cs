using System;
using SimpleGantt.Domain.Interfaces;

namespace SimpleGantt.Domain.ValueObjects;

public record Percentage : IValueObject<double>
{
    public double Value { get; }

    private Percentage()
    {

    }

    public Percentage(double value)
    {
        if (value is < 0 or > 100)
        {
            throw new ArgumentOutOfRangeException("Percentage must be in range from 0 to 100");
        }

        Value = value;
    }

    public static implicit operator double(Percentage percentage) => percentage.Value;
    public static implicit operator Percentage(double value) => new(value);
}

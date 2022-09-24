using System;
using System.Diagnostics;

namespace SimpleGantt.Domain.ValueObjects;

public record Percentage
{
    private double _value;

    public Percentage(double value)
    {
        if (value is < 0 or > 100)
        {
            throw new ArgumentOutOfRangeException("Percentage must be in range from 0 to 100");
        }

        _value = value;
    }

    public static implicit operator double(Percentage percentage)
    {
        return percentage._value;
    }

    public static implicit operator Percentage(double value)
    {
        return new Percentage(value);
    }
}

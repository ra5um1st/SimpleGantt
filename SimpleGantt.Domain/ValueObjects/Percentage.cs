using System;
using System.Diagnostics;

namespace SimpleGantt.Domain.ValueObjects;

public record Percentage
{
    public Percentage(double value)
    {
        Value = value;
    }

    #region Value Property

    private double _value;

    public double Value
    {
        get => _value;
        init
        {
            if (value is < 0 or > 100)
            {
                throw new ArgumentOutOfRangeException("Percentage must be in range from 0 to 100");
            }

            _value = value;
        }
    }

    #endregion

    #region Implicit Convertions

    public static implicit operator double(Percentage percentage)
    {
        return percentage.Value;
    }

    public static implicit operator Percentage(double value)
    {
        return new Percentage(value);
    }

    #endregion
}

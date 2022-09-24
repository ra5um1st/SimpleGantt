using System;

namespace SimpleGantt.Domain.ValueObjects;

public record CurrencyAbbreviation
{
    private string _value;
    private int _maxLength = 3;

    public CurrencyAbbreviation(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            throw new ArgumentNullException(nameof(value));
        }
        if(value.Length >= _maxLength)
        {
            throw new ArgumentException($"Currency abbreviation must be less than or equal {_maxLength}");
        }
        if (!IsUppercase(value))
        {
            throw new ArgumentException("Currency abbreviation must be in uppercase");
        }
        if (!IsEnglishASCII(value))
        {
            throw new ArgumentException("Currency abbreviation must be english ASCII");
        }

        _value = value;
    }

    private bool IsUppercase(string value)
    {
        for (int i = 0; i < value.Length; i++)
        {
            if (!char.IsUpper(value[i]))
            {
                return false;
            }
        }

        return true;
    }

    private bool IsEnglishASCII(string value)
    {
        for (int i = 0; i < value.Length; i++)
        {
            if (!(value[i] >= 'A' && value[i] <= 'Z'))
            {
                return false;
            }
        }

        return true;
    }

    public static implicit operator string(CurrencyAbbreviation currencyAbbreviation)
    {
        return currencyAbbreviation._value;
    }

    public static implicit operator CurrencyAbbreviation(string value)
    {
        return new CurrencyAbbreviation(value);
    }
}

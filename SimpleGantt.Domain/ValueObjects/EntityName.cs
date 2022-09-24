using System;

namespace SimpleGantt.Domain.ValueObjects;

public record class EntityName
{
    private string _value;
    private int _maxLength = 1000;

    public EntityName(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            throw new ArgumentNullException(nameof(value));
        }
        if(value.Length >= _maxLength)
        {
            throw new ArgumentException($"Length of Entity Name can not be more than {_maxLength}");
        }

        _value = value;
    }

    public static implicit operator string(EntityName entityName)
    {
        return entityName._value;
    }

    public static implicit operator EntityName(string value)
    {
        return new EntityName(value);
    }
}

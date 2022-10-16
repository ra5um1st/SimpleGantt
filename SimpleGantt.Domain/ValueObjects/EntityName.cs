using System;
using SimpleGantt.Domain.Interfaces;

namespace SimpleGantt.Domain.ValueObjects;

public record class EntityName : IValueObject<string>
{
    public string Value { get; }
    private const int _maxLength = 1000;

    public EntityName(string value)
    {
        if (value == null)
        {
            throw new ArgumentNullException(nameof(value));
        }
        if(value.Length >= _maxLength)
        {
            throw new ArgumentException($"Length of Entity Name can not be more than {_maxLength}");
        }

        Value = value;
    }


    public static implicit operator string(EntityName entityName) => entityName.Value;
    public static implicit operator EntityName(string value) => new(value);
}

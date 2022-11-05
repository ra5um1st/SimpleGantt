using System;
using SimpleGantt.Domain.Interfaces;

namespace SimpleGantt.Domain.ValueObjects;

public record class EntityName : IValueObject<string>
{
    public string Value { get; }
    public const int MaxLength = 1000;

    public EntityName(string value)
    {
        if (value == null)
        {
            throw new ArgumentNullException(nameof(value));
        }
        if(value.Length >= MaxLength)
        {
            throw new ArgumentException($"Length of Entity Name can not be more than {MaxLength}");
        }

        Value = value;
    }


    public static implicit operator string(EntityName entityName) => entityName.Value;
    public static implicit operator EntityName(string value) => new(value);
}

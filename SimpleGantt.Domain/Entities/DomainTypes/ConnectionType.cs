using System;
using SimpleGantt.Domain.ValueObjects;

namespace SimpleGantt.Domain.Entities;

public record ConnectionType(EntityName Name) : DomainType(Name)
{
    public const string StartStart = "StartStart";
    public const string StartFinish = "StartFinish";
    public const string FinishStart = "FinishStart";
    public const string FinishFinish = "FinishFinish";

    public static implicit operator string(ConnectionType connectionType) => connectionType.Name;
    public static implicit operator ConnectionType(string value) => new(new EntityName(value));
}
using System;

namespace SimpleGantt.Domain.Interfaces;

public interface ITrackable
{
    public DateTimeOffset CreatedAt { get; }
    public DateTimeOffset UpdatedAt { get; }
}

using SimpleGantt.Domain.ValueObjects;

namespace SimpleGantt.Domain.Entities;

public record ConnectionType(EntityName Name) : DomainType(Name)
{
    public static ConnectionType StartStart { get; } = new(1, nameof(StartStart));
    public static ConnectionType StartFinish { get; } = new(2, nameof(StartFinish));
    public static ConnectionType FinishStart { get; } = new(3, nameof(FinishStart));
    public static ConnectionType FinishFinish { get; } = new(4, nameof(FinishFinish));

    private ConnectionType(long id, EntityName name) : this(name)
    {
        Id = id;
    }

    public static implicit operator string(ConnectionType connectionType) => connectionType.Name;
}
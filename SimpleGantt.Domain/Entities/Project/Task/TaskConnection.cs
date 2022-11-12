using System;
using SimpleGantt.Domain.Entities.DomainTypes;

namespace SimpleGantt.Domain.Entities;

public class TaskConnection : Entity
{
    public Task Parent { get; private set; }
    public Task Child { get; private set; }
    public ConnectionType ConnectionType { get; private set; }

    private TaskConnection() : base(default)
    {

    }

    public TaskConnection(Guid id, Task parent, Task child, ConnectionType connectionType) : base(id)
    {
        Parent = parent ?? throw new ArgumentNullException(nameof(parent));
        Child = child ?? throw new ArgumentNullException(nameof(child));
        ConnectionType = connectionType ?? throw new ArgumentNullException(nameof(connectionType));

        ApplyConnection();
    }

    internal void ApplyConnection() => ConnectionType.ApplyConnection(this);
}

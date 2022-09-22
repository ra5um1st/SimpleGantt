using SimpleGantt.Domain.Entities.Abstractions;
using SimpleGantt.Domain.Entities.DomainTypes;

namespace SimpleGantt.Domain.Entities.Common;

public class TaskConnection : Entity
{
    public Task Parent { get; private set; }
    public Task Child { get; private set; }
    public ConnectionType ConnectionType { get; private set; }

    public TaskConnection(Task parent, Task child, ConnectionType connectionType)
    {
        Parent = parent;
        Child = child;
        ConnectionType = connectionType;
    }
}

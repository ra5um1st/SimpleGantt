using SimpleGantt.Domain.Entities.Base;
using SimpleGantt.Domain.Entities.DomainTypes;

namespace SimpleGantt.Domain.Entities.Common;

public class TaskConnection : Entity
{
    public Task Parent { get; set; }
    public Task Child { get; set; }
    public ConnectionType ConnectionType { get; set; }

    public TaskConnection(Task parent, Task child, ConnectionType connectionType)
    {
        Parent = parent;
        Child = child;
        ConnectionType = connectionType;
    }
}

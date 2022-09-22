using SimpleGantt.Domain.Entities.Abstractions;

namespace SimpleGantt.Domain.Entities.Common;

public class TaskHierarchy : Entity
{
    public TaskHierarchy(Task parent, Task child)
    {
        Parent = parent;
        Child = child;
    }

    public Task Parent { get; private set; }
    public Task Child { get; private set; }
}

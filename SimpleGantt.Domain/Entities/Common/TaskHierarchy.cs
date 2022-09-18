using SimpleGantt.Domain.Entities.Base;

namespace SimpleGantt.Domain.Entities.Common;

public class TaskHierarchy : Entity
{
    public TaskHierarchy(Task parent, Task child)
    {
        Parent = parent;
        Child = child;
    }

    public Task Parent { get; set; }
    public Task Child { get; set; }
}

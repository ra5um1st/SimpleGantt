namespace SimpleGantt.Domain.Entities;

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

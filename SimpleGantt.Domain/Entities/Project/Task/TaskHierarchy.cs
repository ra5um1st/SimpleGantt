using System;
using SimpleGantt.Domain.Entities;

namespace SimpleGantt.Domain.Entities;

public class TaskHierarchy : Entity
{
    public Task Parent { get; private set; }
    public Task Child { get; private set; }

    private TaskHierarchy() : base(default)
    {

    }

    public TaskHierarchy(Guid id, Task parent, Task child) : base(id)
    {
        Parent = parent;
        Child = child;
    }
}

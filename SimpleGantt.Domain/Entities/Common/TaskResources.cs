using System;
using SimpleGantt.Domain.Exceptions;

namespace SimpleGantt.Domain.Entities.Common;

public sealed class TaskResource : Entity
{
    public Task Task { get; private set; }
    public Resource Resource { get; private set; }
    public uint Count { get; private set; }

    internal TaskResource(Guid id, Task task, Resource resource, uint count) : base(id)
    {
        Task = task;
        Resource = resource;
        Count = count;
    }

    internal void ChangeResourceAmount(uint count)
    {
        if (count > Count) throw new DomainException("Cannot remove resource. Task have not resource amount enought.");
        if (count > Resource.Count) throw new DomainException("Cannot add resource. Have not resource amount enought.");

        Count += count;
    }
}

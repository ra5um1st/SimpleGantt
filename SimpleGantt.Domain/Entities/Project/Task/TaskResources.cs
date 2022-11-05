using System;
using SimpleGantt.Domain.Exceptions;
using SimpleGantt.Domain.Queries;

namespace SimpleGantt.Domain.Entities;

public sealed class TaskResource : Entity
{
    public Task Task { get; private set; }
    public Resource Resource { get; private set; }
    public double Count { get; private set; }

    internal TaskResource(Guid id, Task task, Resource resource, double count) : base(id)
    {
        Task = task;
        Resource = resource;
        Count = count;
    }

    internal void AddResourceAmount(double amount)
    {
        var alreadyAdded = Task.Project.GetUsedResourcesAmountById(Resource.Id);

        if (amount > Resource.Count - alreadyAdded)
        {
            throw new DomainException($"Cannot add resource to task in the amount of {Count}. " +
                $"To the project tasks have already been added {alreadyAdded}");
        }
        if (amount <= 0) throw new DomainException("Cannot add negative amount of resource");
        if (amount > Resource.Count) throw new DomainException("Resource have not enough resource amount.");

        Count += amount;
    }

    internal void RemoveResourceAmount(double amount)
    {
        if (amount <= 0) throw new DomainException("Cannot remove negative amount of resource");
        if (amount > Count) throw new DomainException("Cannot remove resource from task. Task resource have not enough resource amount.");
        if (Count - amount <= 0) throw new DomainException($"Cannot remove resource from task amount equal to {amount}. Remainder must be non-negative.");

        Count -= amount;
    }
}

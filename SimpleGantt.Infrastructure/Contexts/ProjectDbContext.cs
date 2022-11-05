using System.Reflection;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SimpleGantt.Domain.Entities;

namespace SimpleGantt.Infrastructure.Contexts;

public class ProjectDbContext : DbContext
{
    private readonly IMediator _mediator;

    public ProjectDbContext(IMediator mediator)
    {
        _mediator = mediator;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var aggregateRoots = ChangeTracker
            .Entries<AggregateRoot>()
            .Where(entry => entry.Entity.DomainEvents.Any())
            .Select(entry => entry.Entity);

        var domainEvents = aggregateRoots
            .SelectMany(item => item.DomainEvents)
            .ToList();

        var tasks = new List<System.Threading.Tasks.Task>();

        foreach (var aggregateRoot in aggregateRoots)
        {
            aggregateRoot.ClearDomainEvents();
        }

        foreach (var domainEvent in domainEvents)
        {
            tasks.Add(_mediator.Publish(domainEvent, cancellationToken));
        }

        await System.Threading.Tasks.Task.WhenAll(tasks).ConfigureAwait(false);

        return await base.SaveChangesAsync(cancellationToken);
    }
}

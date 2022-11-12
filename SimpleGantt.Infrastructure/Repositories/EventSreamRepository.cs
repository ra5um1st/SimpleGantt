using Microsoft.EntityFrameworkCore;
using SimpleGantt.Domain.Entities;
using SimpleGantt.Infrastructure.EntityFramework.Contexts;
using SimpleGantt.Domain;
using MediatR;
using SimpleGantt.Domain.Events;
using Task = System.Threading.Tasks.Task;

namespace SimpleGantt.Infrastructure.EntityFramework.Repositories;

public class EventSreamRepository<T> : IEventStreamRepository<T> where T : AggregateRoot, new()
{
    private readonly IMediator _mediator;
    private readonly ProjectDbContext _dbContext;

    public EventSreamRepository(IMediator mediator, ProjectDbContext dbContext)
    {
        _mediator = mediator;
        _dbContext = dbContext;
    }

    public async Task<T> LoadAsync(Guid aggregateId, long version = 0, CancellationToken cancellationToken = default)
    {
        var eventSream = await _dbContext.EventStream
            .Where(item => item.AggregateId == aggregateId && item.Version <= version)
            .Select(item => ValueTuple.Create(item.Data, item.EventType))
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        var aggregate = await eventSream
            .ToAggregateAsync<T>(cancellationToken)
            .ConfigureAwait(false);

        return aggregate;
    }

    public async Task<bool> SaveAsync(T aggregate, CancellationToken cancellationToken = default)
    {
        var eventSream = await aggregate.ToEventStreamAsync(cancellationToken).ConfigureAwait(false);
        await _dbContext.EventStream.AddRangeAsync(eventSream, cancellationToken).ConfigureAwait(false);
        var published = new List<Task>();

        //foreach (var domainEvent in aggregate.DomainEvents)
        //{
        //    published.Add(_mediator.Publish(domainEvent, cancellationToken));
        //}

        await Task.WhenAll(published).ConfigureAwait(false);
        aggregate.ClearDomainEvents();
        var savedCount = await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        return savedCount > 0;
    }
}

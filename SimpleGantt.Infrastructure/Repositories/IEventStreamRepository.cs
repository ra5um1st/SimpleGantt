using SimpleGantt.Domain.Entities;

namespace SimpleGantt.Infrastructure.EntityFramework.Repositories;

public interface IEventStreamRepository<T> where T : AggregateRoot, new()
{
    Task<T> LoadAsync(Guid aggregateId, long version = 0, CancellationToken cancellationToken = default);
    Task<bool> SaveAsync(T aggregate, CancellationToken cancellationToken = default);
}

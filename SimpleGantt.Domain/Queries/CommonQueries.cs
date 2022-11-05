using System;
using System.Linq;
using System.Collections.Generic;
using SimpleGantt.Domain.Entities;

namespace SimpleGantt.Domain.Queries;

public static class CommonQueries
{
    public static TEntity GetEntityById<TEntity>(IEnumerable<TEntity> entities, Guid id)
        where TEntity : Entity => entities.Single(item => item.Id == id);

    public static bool HasEntityWithId<TEntity>(IEnumerable<TEntity> entities, Guid id)
        where TEntity : Entity => entities.Any(item => item.Id == id);
}
using SimpleGantt.Domain.Events.Abstractions;
using System.Collections.Generic;

namespace SimpleGantt.Domain.Interfaces;

public interface IRestoreFromEvents<T> where T : IHasDomainEvents
{
    public T RestoreFromEvents(IEnumerable<DomainEvent> events);
}

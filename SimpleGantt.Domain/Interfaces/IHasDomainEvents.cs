using System.Collections.Generic;
using SimpleGantt.Domain.Entities.Abstractions;
using SimpleGantt.Domain.Events.Abstractions;

namespace SimpleGantt.Domain.Interfaces;

public interface IHasDomainEvents
{
    public IReadOnlyCollection<DomainEvent> DomainEvents { get; }
    protected void AddDomainEvent(DomainEvent @event);
    public void ClearDomainEvents(); 
}

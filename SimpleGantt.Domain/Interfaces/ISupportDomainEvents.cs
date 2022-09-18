using SimpleGantt.Domain.Events.Abstractions;

namespace SimpleGantt.Domain.Interfaces;

public interface ISupportDomainEvents
{
    public bool AddDomainEvent(DomainEvent @event);
    public void ClearDomainEvents(); 
}

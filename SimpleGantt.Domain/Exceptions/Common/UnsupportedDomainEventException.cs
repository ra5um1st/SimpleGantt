using System;

namespace SimpleGantt.Domain.Exceptions.Common;

public class UnsupportedDomainEventException : Exception
{
    public UnsupportedDomainEventException(Type entity, Type domainEvent) : base($"{entity.Name} is not support {domainEvent.Name}.")
    {

    }
}

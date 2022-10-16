using System;

namespace SimpleGantt.Domain.Exceptions;

public class UnsupportedDomainEventException : Exception
{
    public UnsupportedDomainEventException(string message) : base(message)
    {

    }

    public UnsupportedDomainEventException(Type entity, Type domainEvent) : base($"{entity.Name} does not support {domainEvent.Name}")
    {

    }
}

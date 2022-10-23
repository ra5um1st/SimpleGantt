using System;

namespace SimpleGantt.Domain.Exceptions;

public class DomainEventException : Exception
{
    public DomainEventException(string eventName) : base($"Wrong data from {eventName}")
    {

    }
}

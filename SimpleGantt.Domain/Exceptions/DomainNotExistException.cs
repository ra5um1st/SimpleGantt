using System;

namespace SimpleGantt.Domain.Exceptions;

public class DomainNotExistException : Exception
{
    public DomainNotExistException(string entityName) : base($"Entity {entityName} does not exist")
    {

    }
}

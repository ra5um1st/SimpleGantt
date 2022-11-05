using System;

namespace SimpleGantt.Domain.Exceptions;

public class DomainNotExistException : Exception
{
    public DomainNotExistException(string entityName) : base($"Entity {entityName} does not exist") { }
    public DomainNotExistException(Guid id) : base($"Entity with id {id} does not exist") { }
}

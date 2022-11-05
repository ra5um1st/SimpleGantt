using System;

namespace SimpleGantt.Domain.Exceptions;

public class DomainExistsException : Exception
{
    public DomainExistsException(string entityName) : base($"Entity {entityName} already exists") { }
    public DomainExistsException(Guid id) : base($"Entity with {id} already exists") { }

}

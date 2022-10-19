using System;

namespace SimpleGantt.Domain.Exceptions;

public class EntityExistsException : Exception
{
    public EntityExistsException(string entityName) : base($"Entity {entityName} already exists") 
    {

    }
}

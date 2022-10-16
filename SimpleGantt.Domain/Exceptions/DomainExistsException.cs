using System;
using SimpleGantt.Domain.Entities;

namespace SimpleGantt.Domain.Exceptions;

public class DomainExistsException : Exception
{
    public DomainExistsException(string message) : base(message)
    {

    }

    public DomainExistsException() : base($"Domain already exists") 
    {

    }

    public DomainExistsException(Task parent, Task child, ConnectionType connectionType)
        : base($"Task connection of type {connectionType.Name} already exists within Task parentId = {parent.Id} and Task childId = {child.Id}")
    {

    }

    public DomainExistsException(Task parent, Task child)
        : base($"Task connection already exists within Task parentId = {parent.Id} and Task childId = {child.Id}")
    {

    }
}

using System;
using SimpleGantt.Domain.Entities;

namespace SimpleGantt.Domain.Exceptions;

public class DomainNotExistException : Exception
{
    public DomainNotExistException(string message) : base(message)
    {

    }

    public DomainNotExistException(Task parent, Task child)
        : base($"Can not remove task connection from Task parentId = {parent.Id} and Task childId = {child.Id}")
    {

    }
}

using System;
using SimpleGantt.Domain.Entities.Common;
using SimpleGantt.Domain.Entities.DomainTypes;

namespace SimpleGantt.Domain.Exceptions.Common;

public class AlreadyExistsException : Exception
{
    public AlreadyExistsException(Task parent, Task child, ConnectionType connectionType)
        : base($"Task connection of type {connectionType.Name} already exists within Task parentId = {parent.Id} and Task childId = {child.Id}")
    {

    }

    public AlreadyExistsException(Task parent, Task child)
    : base($"Task connection already exists within Task parentId = {parent.Id} and Task childId = {child.Id}")
    {

    }
}

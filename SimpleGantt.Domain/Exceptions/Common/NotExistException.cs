using System;
using SimpleGantt.Domain.Entities.Common;

namespace SimpleGantt.Domain.Exceptions.Common;

public class NotExistException : Exception
{
    public NotExistException(Task parent, Task child)
        : base($"Can not remove task connection from Task parentId = {parent.Id} and Task childId = {child.Id}")
    {

    }
}

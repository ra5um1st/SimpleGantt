using System.Linq;
using SimpleGantt.Domain.Entities;
using SimpleGantt.Domain.Entities.DomainTypes;

namespace SimpleGantt.Domain.Queries;

public static class TaskQueries
{
    public static bool HasConnectionOfType(this Task task, ConnectionType connectionType)
    {
        return task.Connections
                    .Select(item => item.ConnectionType.Name)
                    .Any(item => item == connectionType.Name);
    }

    public static bool HasConnectionWithChild(this Task task, Task child)
    {
        return task.Connections
                    .Select(item => item.Child.Id)
                    .Any(item => item == child.Id);
    }

    public static bool HasConnectionWithChildOfType(this Task task, Task child, ConnectionType connectionType)
    {
        return task.Connections
                    .Select(item => new { item.Child.Id, ConnectionTypeName = item.ConnectionType.Name })
                    .Any(item => item.Id == child.Id && item.ConnectionTypeName == connectionType.Name);
    }

    public static bool HasConnectionWithParentOfTYpe(this Task task, Task parent, ConnectionType connectionType)
    {
        return task.Connections
                    .Select(item => new { item.Parent.Id, ConnectionTypeName = item.ConnectionType.Name })
                    .Any(item => item.Id == parent.Id && item.ConnectionTypeName == connectionType.Name);
    }

    public static TaskConnection GetConnectionByChild(this Task task, Task child)
    {
        return task.Connections.First(item => item.Parent.Id == task.Id && item.Child.Id == child.Id);
    }

    public static bool HasSubtask(this Task task, Task child)
    {
        return task.Hierarchy.Any(item => item.Parent.Id == task.Id && item.Child.Id == child.Id);
    }
}

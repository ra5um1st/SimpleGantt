using SimpleGantt.Domain.Entities.DomainTypes;

namespace SimpleGantt.Domain.Tests;

public class ProjectTests
{
    private static readonly DateTimeOffset Now = DateTimeOffset.Now;

    public static readonly object[][] CreateTasksData =
    {
        // Correct case
        new object[]
        {
            new List<TaskCreated>()
            {
                new TaskCreated(Guid.Empty, Guid.NewGuid(), "Task 1", Now, Now.AddMinutes(30), 90),
                new TaskCreated(Guid.Empty, Guid.NewGuid(), "Task 2", Now.AddHours(-5), Now.AddHours(30), 30),
                new TaskCreated(Guid.Empty, Guid.NewGuid(), "Task 3", Now.AddMinutes(10), Now.AddMinutes(60), 0)
            },
        },
    };

    [Theory]
    [MemberData(nameof(CreateTasksData))]
    public void RestoreFromEvents_CreateTasks_RestoredTasksShouldBeEqual(IEnumerable<TaskCreated> events)
    {
        var originalProject = Project.Create(new ProjectCreated(Guid.Empty, "Test project", DateTimeOffset.Now));

        foreach (var @event in events)
        {
            originalProject.CreateTask(@event);
        }

        var restoredProject = AggregateRoot.RestoreFrom<Project>(originalProject.DomainEvents);

        Assert.Equal(originalProject.Name, restoredProject.Name);
        Assert.Equal(originalProject.Version, restoredProject.Version);
        Assert.Equal(originalProject.Removed, restoredProject.Removed);

        for (var i = 0; i < originalProject.Tasks.Count; i++)
        {
            var originalTask = originalProject.Tasks.ElementAt(i);
            var restoredTask = restoredProject.Tasks.ElementAt(i);

            Assert.Equal(originalTask.Name, restoredTask.Name);
            Assert.Equal(originalTask.StartDate, restoredTask.StartDate);
            Assert.Equal(originalTask.FinishDate, restoredTask.FinishDate);
            Assert.Equal(originalTask.CompletionPercentage, restoredTask.CompletionPercentage);
        }
    }

    private static readonly Guid MainTaskId = Guid.NewGuid();
    private static readonly Guid ChildTaskId = Guid.NewGuid();

    public static readonly object[][] CreateStartStartConnectionData =
    {
        // Correct case
        new object[]
        {
            new TaskConnectionAdded(Guid.NewGuid(), MainTaskId, ChildTaskId, ConnectionType.StartStart),
        },
    };

    [Theory]
    [MemberData(nameof(CreateStartStartConnectionData))]
    public void RestoreFromEvents_CreateStartStartConnection_RestoredTasksShouldBeEqual(TaskConnectionAdded taskConnectionAdded)
    {
        var originalProject = Project.Create(new ProjectCreated(Guid.Empty, "Test project", DateTimeOffset.Now));
        var mainTask = originalProject.CreateTask(new TaskCreated(Guid.Empty, MainTaskId, "Task 1", Now.AddMinutes(10), Now.AddHours(1), 50));
        var childTask = originalProject.CreateTask(new TaskCreated(Guid.Empty, ChildTaskId, "Task 2", Now, Now.AddHours(2), 1));
        originalProject.AddTaskConnection(taskConnectionAdded);

        var restoredProject = AggregateRoot.RestoreFrom<Project>(originalProject.DomainEvents);

        Assert.Equal(originalProject.Name, restoredProject.Name);
        Assert.Equal(originalProject.Version, restoredProject.Version);
        Assert.Equal(originalProject.Removed, restoredProject.Removed);

        for (var i = 0; i < originalProject.Tasks.Count; i++)
        {
            var originalTask = originalProject.Tasks.ElementAt(i);
            var restoredTask = restoredProject.Tasks.ElementAt(i);

            Assert.Equal(originalTask.Name, restoredTask.Name);
            Assert.Equal(originalTask.StartDate, restoredTask.StartDate);
            Assert.Equal(originalTask.FinishDate, restoredTask.FinishDate);
            Assert.Equal(originalTask.CompletionPercentage, restoredTask.CompletionPercentage);
        }
    }
}

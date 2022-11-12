
using SimpleGantt.Domain.Events;

namespace SimpleGantt.Domain.Tests;

public class TaskTests
{
    private static readonly DateTimeOffset Now = DateTimeOffset.Now;

    [Fact]
    public void AddTask_CorrectTask_ShouldAdd()
    {
        var project = Project.Create(new ProjectCreated(Guid.NewGuid(), "test project", DateTimeOffset.Now));
        var task1 = project.CreateTask(new TaskCreated(project.Id, Guid.NewGuid(), "task 1", DateTimeOffset.Now, DateTimeOffset.Now.AddHours(1), 50));

        Assert.Contains(task1, project.Tasks);
    }

    [Fact]
    public void AddTask_CorrectTaskWithExistingId_ShouldThrowException()
    {
        var project = Project.Create(new ProjectCreated(Guid.NewGuid(), "test project", DateTimeOffset.Now));
        var id = Guid.NewGuid();
        var task1 = project.CreateTask(new TaskCreated(project.Id, id, "task 1", DateTimeOffset.Now, DateTimeOffset.Now.AddHours(1), 50));

        Assert.Throws<DomainExistsException>(() => project.CreateTask(new TaskCreated(project.Id, id, "task 2", DateTimeOffset.Now, DateTimeOffset.Now.AddHours(2), 51)));
    }

    [Fact]
    public void RemoveTask_CorrectTask_ShouldRemove()
    {
        var project = Project.Create(new ProjectCreated(Guid.NewGuid(), "test project", DateTimeOffset.Now));
        var task1 = project.CreateTask(new TaskCreated(project.Id, Guid.NewGuid(), "task 1", DateTimeOffset.Now, DateTimeOffset.Now.AddHours(1), 50));
        project.RemoveTask(new TaskRemoved(project.Id, task1.Id));

        Assert.DoesNotContain(task1, project.Tasks);
    }

    [Fact]
    public void RemoveTask_NotExistingTask_ShouldThrowException()
    {
        var project = Project.Create(new ProjectCreated(Guid.NewGuid(), "test project", DateTimeOffset.Now));

        Assert.Throws<DomainNotExistException>(() => project.RemoveTask(new TaskRemoved(project.Id, Guid.NewGuid())));
    }
}

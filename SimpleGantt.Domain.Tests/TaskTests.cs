using static SimpleGantt.Domain.Events.Common.TaskEvents;

namespace SimpleGantt.Domain.Tests;

public class TaskTests
{
    [Fact]
    public void RestoreFrom_TaskCreatedEvent_ReturnsSameTask()
    {
        var project = new Project(Guid.NewGuid(), "Test project", DateTimeOffset.Now);
        var task1 = project.CreateTask(new TaskCreated(project.Id, Guid.NewGuid(), "test task 1", DateTime.Now, DateTime.Now.AddHours(1), new Percentage(50)));

        var task2 = Task.RestoreFrom(task1.DomainEvents);

        Assert.Equal(task1.Name, task2.Name);
        Assert.Equal(task1.StartDate, task2.StartDate);
        Assert.Equal(task1.FinishDate, task2.FinishDate);
        Assert.Equal(task1.CompletionPercentage, task2.CompletionPercentage);
    }

    [Fact]
    public void RestoreFrom_MultipleEvents_ReturnsSameTask()
    {
        var project = new Project(Guid.NewGuid(), "Test project", DateTimeOffset.Now);
        var task1 = project.CreateTask(new TaskCreated(project.Id, Guid.NewGuid(), "test task 1", DateTime.Now, DateTime.Now.AddHours(1), new Percentage(50)));

        task1.ChangeStartDate(task1.StartDate.AddMinutes(20));
        task1.ChangeFinishDate(task1.FinishDate.AddMinutes(-20));
        task1.ChangeCompletionPercentage(11);
        task1.ChangeName("qwerty");

        var task2 = Task.RestoreFrom(task1.DomainEvents);

        Assert.Equal(task1.Name, task2.Name);
        Assert.Equal(task1.StartDate, task2.StartDate);
        Assert.Equal(task1.FinishDate, task2.FinishDate);
        Assert.Equal(task1.CompletionPercentage, task2.CompletionPercentage);
    }
}
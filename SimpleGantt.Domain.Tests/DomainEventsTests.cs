using Task = SimpleGantt.Domain.Entities.Task;

namespace SimpleGantt.Domain.Tests;

public class DomainEventsTests
{
    [Fact]
    public void Task_RestoreFromEvents_ShouldWork()
    {
        var project = new Project("Test project", DateTimeOffset.Now);
        var task1 = new Task(project, "test task 1", DateTime.Now, DateTime.Now.AddHours(1), new Percentage(50));
        task1.ChangeStartDate(task1.StartDate.AddMinutes(20));
        task1.ChangeFinishDate(task1.FinishDate.AddMinutes(-20));
        task1.ChangeName("qwerty");

        var task2 = Task.RestoreFromEvents(task1.DomainEvents);

        Assert.Equal(task1.Name, task2.Name);
        Assert.Equal(task1.StartDate, task2.StartDate);
        Assert.Equal(task1.FinishDate, task2.FinishDate);
        Assert.Equal(task1.CompletionPercentage, task2.CompletionPercentage);
    }
}
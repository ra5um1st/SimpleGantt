using Task = SimpleGantt.Domain.Entities.Common.Task;

namespace SimpleGantt.Domain.Tests;

public class DomainEventsTests
{
    [Fact]
    public void Task_ModifyAndRestoreFromEvents()
    {
        Task task1 = new Task("test task 1", DateTime.Now, DateTime.Now.AddHours(1), new Percentage(50));
        task1.ChangeStartDate(task1.StartDate.AddMinutes(20));

        Task task2 = Task.RestoreFromEvents1(task1.DomainEvents);

        Assert.Equal(task1.Name, task2.Name);
        Assert.Equal(task1.StartDate, task2.StartDate);
        Assert.Equal(task1.FinishDate, task2.FinishDate);
        Assert.Equal(task1.CompletionPercentage, task2.CompletionPercentage);
    }
}
using SimpleGantt.Domain.Factories;
using Task = SimpleGantt.Domain.Entities.Common.Task;
using TaskFactory = SimpleGantt.Domain.Factories.TaskFactory;

namespace SimpleGantt.Domain.Tests;

public class DomainEventsTests
{
    [Fact]
    public void Task_Create_Modify_RestoreFromEvents()
    {
        Task task1 = new Task("test task 1", DateTime.Now, DateTime.Now.AddHours(1), new Percentage(50));
        task1.ChangeStartDate(task1.StartDate.AddMinutes(20));

        TaskFactory taskFactory = new TaskFactory();
        Task task2 = taskFactory.RestoreFromEvents(task1.DomainEvents);

        Assert.Equal(task1.Name, task2.Name);
        Assert.Equal(task1.StartDate, task2.StartDate);
        Assert.Equal(task1.FinishDate, task2.FinishDate);
        Assert.Equal(task1.CompletionPercentage, task2.CompletionPercentage);
    }
}
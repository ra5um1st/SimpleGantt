using SimpleGantt.Domain.Events;
using static SimpleGantt.Domain.Events.Common.TaskEvents;

namespace SimpleGantt.Domain.Tests.Project;

public class AddAndChangeTasks
{
    private static readonly DateTimeOffset Now = DateTimeOffset.Now;
    private static readonly Guid Task1Id = Guid.NewGuid();
    private static readonly Guid Task2Id = Guid.NewGuid();
    private static readonly Guid Task3Id = Guid.NewGuid();

    public static readonly object[][] AddAndChangeTasksData =
    {
        // Correct case
        new object[]
        {
            new List<DomainEvent>()
            {
                new TaskCreated(Guid.Empty, Task1Id, "Task 1", Now, Now.AddMinutes(30), 90),
                new TaskStartDateChanged(Task1Id, Now.AddMinutes(10)),
                new TaskNameChanged(Task1Id, "Task 1 renamed"),
                new TaskCreated(Guid.Empty, Task2Id, "Task 2", Now.AddHours(-5), Now.AddHours(30), 30),
                new TaskFinishDateChanged(Task2Id, Now.AddMinutes(10)),
                new TaskCreated(Guid.Empty, Task3Id, "Task 3", Now.AddMinutes(10), Now.AddMinutes(60), 0),
                new TaskCompletionPercentageChanged(Task3Id, 100),
            },
        },
    };

    [Theory]
    [MemberData(nameof(AddAndChangeTasksData))]
    public void RestoreFrom_AddAndChangeTasks_ReturnsSameTask(IEnumerable<DomainEvent> events)
    {
        var originalProject = new Entities.Common.Project(Guid.Empty, "Test project", DateTimeOffset.Now);

        foreach (var @event in events)
        {
            switch (@event)
            {
                case TaskCreated taskCreated:
                    originalProject.CreateTask(taskCreated);
                    break;
                case TaskRemoved taskRemoved:
                    originalProject.RemoveTask(taskRemoved);
                    break;
                case TaskStartDateChanged startDateChanged:
                    originalProject.ChangeTaskStartDate(startDateChanged);
                    break;
                case TaskFinishDateChanged finishDateChanged:
                    originalProject.ChangeTaskFinishDate(finishDateChanged);
                    break;
                case TaskNameChanged taskNameChanged:
                    originalProject.ChangeTaskName(taskNameChanged);
                    break;
                case TaskCompletionPercentageChanged percentageChanged:
                    originalProject.ChangeTaskCompletionPercentage(percentageChanged);
                    break;
                default:
                    throw new Exception();
            }
        }

        var restoredProject = Entities.Common.Project.RestoreFrom(originalProject.DomainEvents);

        Assert.Equal(originalProject.Name, restoredProject.Name);
        Assert.Equal(originalProject.Version, restoredProject.Version);
        Assert.Equal(originalProject.Removed, restoredProject.Removed);

        for (var i = 0; i < originalProject.Tasks.Count; i++)
        {
            var originalTask = originalProject.Tasks.ElementAt(i);
            var restoredTask = restoredProject.Tasks.ElementAt(i);
            Assert.Equal(originalTask.Name, originalTask.Name);
            Assert.Equal(originalTask.StartDate, originalTask.StartDate);
            Assert.Equal(originalTask.FinishDate, originalTask.FinishDate);
            Assert.Equal(originalTask.CompletionPercentage, originalTask.CompletionPercentage);
        }
    }
}

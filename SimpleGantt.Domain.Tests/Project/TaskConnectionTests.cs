using SimpleGantt.Domain.Entities.DomainTypes;

namespace SimpleGantt.Domain.Tests;

public class TaskConnectionTests
{
    private static readonly DateTimeOffset Now = DateTimeOffset.Now;
    private static readonly Guid MainTaskId = Guid.NewGuid();
    private static readonly Guid ChildTaskId = Guid.NewGuid();

    public static readonly object[][] StartStartData =
    {
        // Correct case
        new object[] { Now, Now.AddMinutes(30), Now.AddMinutes(10), Now.AddMinutes(20) },
        // Edge case
        new object[] { Now, Now.AddMinutes(30), Now, Now.AddMinutes(20) },
        // Incorrect case
        new object[] { Now, Now.AddMinutes(30), Now.AddMinutes(-30), Now.AddMinutes(-10) }
    };

    [Theory]
    [MemberData(nameof(StartStartData))]
    public void AddConnection_StartStart_ShouldAdd(
        DateTimeOffset task1StartDate,
        DateTimeOffset task1FinishDate,
        DateTimeOffset task2StartDate,
        DateTimeOffset task2FinishDate)
    {
        var project = Project.Create(new ProjectCreated(Guid.NewGuid(), "test project", DateTimeOffset.Now));

        var task1 = project.CreateTask(new TaskCreated(project.Id, Guid.NewGuid(), "task 1", task1StartDate, task1FinishDate, 50));
        var task2 = project.CreateTask(new TaskCreated(project.Id, Guid.NewGuid(), "task 2", task2StartDate, task2FinishDate, 1));
        var connection = project.AddTaskConnection(new TaskConnectionAdded(Guid.NewGuid(), task1.Id, task2.Id, ConnectionType.StartStart));

        var datesDifference = task1StartDate - task2StartDate;

        if (datesDifference > TimeSpan.Zero)
        {
            Assert.Equal(task1.StartDate, task1StartDate);
            Assert.Equal(task1.FinishDate, task1FinishDate);
            Assert.Equal(task2.StartDate, task1StartDate);
            Assert.Equal(task2.FinishDate, task2FinishDate + datesDifference);
        }
        else
        {
            Assert.Equal(task1.StartDate, task1StartDate);
            Assert.Equal(task1.FinishDate, task1FinishDate);
            Assert.Equal(task2.StartDate, task2StartDate);
            Assert.Equal(task2.FinishDate, task2FinishDate);
        }
    }

    public static readonly object[][] StartFinishData =
    {
        // Correct case
        new object[] { Now, Now.AddMinutes(30), Now.AddMinutes(10), Now.AddMinutes(20) },
        // Edge case
        new object[] { Now, Now.AddMinutes(30), Now.AddMinutes(-30), Now },
        // Incorrect case
        new object[] { Now, Now.AddMinutes(30), Now.AddMinutes(-30), Now.AddMinutes(-10) }
    };

    [Theory]
    [MemberData(nameof(StartFinishData))]
    public void AddConnection_StartFinish_ShouldAdd(
        DateTimeOffset task1StartDate,
        DateTimeOffset task1FinishDate,
        DateTimeOffset task2StartDate,
        DateTimeOffset task2FinishDate)
    {
        var project = Project.Create(new ProjectCreated(Guid.NewGuid(), "test project", DateTimeOffset.Now));
        var tasksStartDate = DateTimeOffset.Now;

        var task1 = project.CreateTask(new TaskCreated(project.Id, Guid.NewGuid(), "task 1", task1StartDate, task1FinishDate, 50));
        var task2 = project.CreateTask(new TaskCreated(project.Id, Guid.NewGuid(), "task 2", task2StartDate, task2FinishDate, 1));
        var connection = project.AddTaskConnection(new TaskConnectionAdded(Guid.NewGuid(), task1.Id, task2.Id, ConnectionType.StartFinish));

        var datesDifference = task1StartDate - task2FinishDate;

        if (datesDifference > TimeSpan.Zero)
        {
            Assert.Equal(task1.StartDate, task1StartDate);
            Assert.Equal(task1.FinishDate, task1FinishDate);
            Assert.Equal(task2.StartDate, task2StartDate + datesDifference);
            Assert.Equal(task2.FinishDate, task1.StartDate);
        }
        else
        {
            Assert.Equal(task1.StartDate, task1StartDate);
            Assert.Equal(task1.FinishDate, task1FinishDate);
            Assert.Equal(task2.StartDate, task2StartDate);
            Assert.Equal(task2.FinishDate, task2FinishDate);
        }
    }

    public static readonly object[][] FinishStartData =
    {
        // Correct case
        new object[] { Now, Now.AddMinutes(30), Now.AddMinutes(40), Now.AddMinutes(60) },
        // Edge case
        new object[] { Now, Now.AddMinutes(30), Now.AddMinutes(30), Now.AddMinutes(60) },
        // Incorrect case
        new object[] { Now, Now.AddMinutes(30), Now.AddMinutes(10), Now.AddMinutes(60) },
    };

    [Theory]
    [MemberData(nameof(FinishStartData))]
    public void AddConnection_FinishStart_ShouldAdd(
        DateTimeOffset task1StartDate,
        DateTimeOffset task1FinishDate,
        DateTimeOffset task2StartDate,
        DateTimeOffset task2FinishDate)
    {
        var project = Project.Create(new ProjectCreated(Guid.NewGuid(), "test project", DateTimeOffset.Now));
        var task1 = project.CreateTask(new TaskCreated(project.Id, Guid.NewGuid(), "task 1", task1StartDate, task1FinishDate, 50));
        var task2 = project.CreateTask(new TaskCreated(project.Id, Guid.NewGuid(), "task 2", task2StartDate, task2FinishDate, 1));
        var connection = project.AddTaskConnection(new TaskConnectionAdded(Guid.NewGuid(), task1.Id, task2.Id, ConnectionType.FinishStart));

        var datesDifference = task1FinishDate - task2StartDate;

        if (datesDifference > TimeSpan.Zero)
        {
            Assert.Equal(task1.StartDate, task1StartDate);
            Assert.Equal(task1.FinishDate, task1FinishDate);
            Assert.Equal(task2.StartDate, task1FinishDate);
            Assert.Equal(task2.FinishDate, task2FinishDate + datesDifference);
        }
        else
        {
            Assert.Equal(task1.StartDate, task1StartDate);
            Assert.Equal(task1.FinishDate, task1FinishDate);
            Assert.Equal(task2.StartDate, task2StartDate);
            Assert.Equal(task2.FinishDate, task2FinishDate);
        }
    }

    public static readonly object[][] FinishFinishData =
{
        // Correct case
        new object[] { Now, Now.AddMinutes(30), Now.AddMinutes(10), Now.AddMinutes(20) },
        // Edge case
        new object[] { Now, Now.AddMinutes(30), Now.AddMinutes(10), Now.AddMinutes(30) },
        // Incorrect case
        new object[] { Now, Now.AddMinutes(30), Now.AddMinutes(10), Now.AddMinutes(40) },
    };

    [Theory]
    [MemberData(nameof(FinishFinishData))]
    public void AddConnection_FinishFinish_ShouldAdd(
        DateTimeOffset task1StartDate,
        DateTimeOffset task1FinishDate,
        DateTimeOffset task2StartDate,
        DateTimeOffset task2FinishDate)
    {
        var project = Project.Create(new ProjectCreated(Guid.NewGuid(), "test project", DateTimeOffset.Now));
        var task1 = project.CreateTask(new TaskCreated(project.Id, Guid.NewGuid(), "task 1", task1StartDate, task1FinishDate, 50));
        var task2 = project.CreateTask(new TaskCreated(project.Id, Guid.NewGuid(), "task 2", task2StartDate, task2FinishDate, 1));
        var connection = project.AddTaskConnection(new TaskConnectionAdded(Guid.NewGuid(), task1.Id, task2.Id, ConnectionType.FinishFinish));

        var datesDifference = task1FinishDate - task2FinishDate;

        if (datesDifference < TimeSpan.Zero)
        {
            Assert.Equal(task1.StartDate, task1StartDate);
            Assert.Equal(task1.FinishDate, task1FinishDate);
            Assert.Equal(task2.StartDate, task2StartDate + datesDifference);
            Assert.Equal(task2.FinishDate, task1FinishDate);
        }
        else
        {
            Assert.Equal(task1.StartDate, task1StartDate);
            Assert.Equal(task1.FinishDate, task1FinishDate);
            Assert.Equal(task2.StartDate, task2StartDate);
            Assert.Equal(task2.FinishDate, task2FinishDate);
        }
    }

    public static readonly object[][] StartDateWithConnectionsChangeData =
    {
        // After changing main task start date child task start date should be equal to it
        new object[]
        {
            Now,
            new TaskConnectionAdded(Guid.NewGuid(), MainTaskId, ChildTaskId, ConnectionType.StartStart),
            new TaskStartDateChanged(MainTaskId, Now.AddMinutes(10)),
        },
        new object[]
        {
            Now.AddMinutes(10),
            new TaskConnectionAdded(Guid.NewGuid(), MainTaskId, ChildTaskId, ConnectionType.StartStart),
            new TaskStartDateChanged(MainTaskId, Now.AddMinutes(10)),
        },
        new object[]
        {
            Now.AddMinutes(20),
            new TaskConnectionAdded(Guid.NewGuid(), MainTaskId, ChildTaskId, ConnectionType.StartStart),
            new TaskStartDateChanged(MainTaskId, Now.AddMinutes(10)),
        },
    };

    [Theory]
    [MemberData(nameof(StartDateWithConnectionsChangeData))]
    public void ChangeStartDate_WithConncetions_ConnectedTasksStartDatesShouldBeEqual(
        DateTimeOffset childTaskStartDate,
        TaskConnectionAdded taskConnectionAdded,
        TaskStartDateChanged taskStartDateChanged)
    {
        var project = Project.Create(new ProjectCreated(Guid.Empty, "Test project", DateTimeOffset.Now));

        var mainTaskStartDate = Now;
        var mainTaskFinishDate = Now.AddHours(1);
        var mainTask = project.CreateTask(new TaskCreated(Guid.Empty, MainTaskId, "Task 1", mainTaskStartDate, mainTaskFinishDate, 90));

        var childTaskFinishDate = childTaskStartDate.AddHours(30);
        var childTask = project.CreateTask(new TaskCreated(Guid.Empty, ChildTaskId, "Task 2", childTaskStartDate, childTaskFinishDate, 30));

        project.AddTaskConnection(taskConnectionAdded);
        project.ChangeTaskStartDate(taskStartDateChanged);

        var datesDifference = taskStartDateChanged.NewStartDate - childTaskStartDate;

        if (datesDifference > TimeSpan.Zero)
        {
            Assert.Equal(childTask.StartDate, mainTask.StartDate);
            Assert.Equal(childTask.FinishDate, childTaskFinishDate + datesDifference);
        }
        else
        {
            Assert.Equal(childTask.StartDate, childTaskStartDate);
            Assert.Equal(childTask.FinishDate, childTaskFinishDate);
        }
    }
}
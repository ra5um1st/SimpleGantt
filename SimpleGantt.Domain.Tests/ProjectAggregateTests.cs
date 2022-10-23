﻿using SimpleGantt.Domain.Entities;
using static SimpleGantt.Domain.Events.Common.ProjectEvents;
using static SimpleGantt.Domain.Events.Common.TaskEvents;

namespace SimpleGantt.Domain.Tests;

public class ProjectAggregateTests
{
    private static readonly DateTimeOffset Now = DateTimeOffset.Now;

    [Fact]
    public void AddTask_CorrectTask_ShouldAddTask()
    {
        var project = new Entities.Common.Project(Guid.NewGuid(), "test project", DateTimeOffset.Now);
        Task task1 = project.CreateTask(new TaskCreated(project.Id, Guid.NewGuid(), "task 1", DateTimeOffset.Now, DateTimeOffset.Now.AddHours(1), 50));

        Assert.Contains(task1, project.Tasks);
    }

    [Fact]
    public void AddConnection_CorrectStartStart_ShouldAdd()
    {
        var project = new Entities.Common.Project(Guid.NewGuid(), "test project", DateTimeOffset.Now);
        DateTimeOffset tasksStartDate = DateTimeOffset.Now;

        Task task1 = project.CreateTask(new TaskCreated(project.Id, Guid.NewGuid(), "task 1", tasksStartDate, DateTimeOffset.Now.AddHours(1), 50));
        Task task2 = project.CreateTask(new TaskCreated(project.Id, Guid.NewGuid(), "task 2", tasksStartDate, DateTimeOffset.Now.AddHours(0.8), 1));
        TaskConnection connection = project.AddConnection(new ConnectionAdded(Guid.NewGuid(), task1.Id, task2.Id, ConnectionType.StartStart));

        Assert.Contains(task1.Connections, connection => connection.ConnectionType == ConnectionType.StartStart);
        Assert.Contains(task2.Connections, connection => connection.ConnectionType == ConnectionType.StartStart);
        Assert.True(task1.HasConnectionWithChild(task2));
    }

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
        var project = new Entities.Common.Project(Guid.NewGuid(), "test project", DateTimeOffset.Now);

        Task task1 = project.CreateTask(new TaskCreated(project.Id, Guid.NewGuid(), "task 1", task1StartDate, task1FinishDate, 50));
        Task task2 = project.CreateTask(new TaskCreated(project.Id, Guid.NewGuid(), "task 2", task2StartDate, task2FinishDate, 1));
        TaskConnection connection = project.AddConnection(new ConnectionAdded(Guid.NewGuid(), task1.Id, task2.Id, ConnectionType.StartStart));

        TimeSpan datesDifference = task1StartDate - task2StartDate;

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
        var project = new Entities.Common.Project(Guid.NewGuid(), "test project", DateTimeOffset.Now);
        DateTimeOffset tasksStartDate = DateTimeOffset.Now;

        Task task1 = project.CreateTask(new TaskCreated(project.Id, Guid.NewGuid(), "task 1", task1StartDate, task1FinishDate, 50));
        Task task2 = project.CreateTask(new TaskCreated(project.Id, Guid.NewGuid(), "task 2", task2StartDate, task2FinishDate, 1));
        TaskConnection connection = project.AddConnection(new ConnectionAdded(Guid.NewGuid(), task1.Id, task2.Id, ConnectionType.StartFinish));

        TimeSpan datesDifference = task1StartDate - task2FinishDate;

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

    public static readonly object[][] AddTasksData =
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
    [MemberData(nameof(AddTasksData))]
    public void RestoreFromEvents_AddTasks_ShouldReturnSameProject(IEnumerable<TaskCreated> events)
    {
        var originalProject = new Entities.Common.Project(Guid.Empty, "Test project", DateTimeOffset.Now);

        foreach (TaskCreated @event in events)
        {
            originalProject.CreateTask(@event);
        }

        var restoredProject = Entities.Common.Project.RestoreFrom(originalProject.DomainEvents);

        Assert.Equal(originalProject.Name, restoredProject.Name);
        Assert.Equal(originalProject.Version, restoredProject.Version);
        Assert.Equal(originalProject.Removed, restoredProject.Removed);

        for (var i = 0; i < originalProject.Tasks.Count; i++)
        {
            Task originalTask = originalProject.Tasks.ElementAt(i);
            Task restoredTask = restoredProject.Tasks.ElementAt(i);
            Assert.Equal(originalTask.Name, originalTask.Name);
            Assert.Equal(originalTask.StartDate, originalTask.StartDate);
            Assert.Equal(originalTask.FinishDate, originalTask.FinishDate);
            Assert.Equal(originalTask.CompletionPercentage, originalTask.CompletionPercentage);
        }
    }
}

using System;
using System.Collections.Generic;
using SimpleGantt.Domain.Interfaces;
using SimpleGantt.Domain.ValueObjects;

namespace SimpleGantt.Domain.Entities;

public partial class Project : AggregateRoot, INamable, ITrackable
{
    private readonly HashSet<Task> _tasks = new();
    private readonly HashSet<Resource> _resources = new();

    private EntityName _name = string.Empty;
    public EntityName Name
    {
        get => _name;
        set => ApplyDomainEvent(new ProjectNameChanged(Id, value));
    }
       
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset UpdatedAt { get; private set; }
    public IReadOnlyCollection<Task> Tasks => _tasks;
    public IReadOnlyCollection<Resource> Resources => _resources;

    public Project() : base(Guid.Empty)
    {

    }

    public static Project Create(ProjectCreated projectCreated)
    {
        var project = new Project();
        project.ApplyDomainEvent(projectCreated);
        return project;
    }

    // TODO: Extract task factory
    public Task CreateTask(TaskCreated taskCreated) => (Task)ApplyDomainEvent(taskCreated);

    public void RemoveTask(TaskRemoved taskRemoved) => ApplyDomainEvent(taskRemoved);

    public void ChangeTaskStartDate(TaskStartDateChanged startDateChanged) => ApplyDomainEvent(startDateChanged);

    public void ChangeTaskFinishDate(TaskFinishDateChanged finishDateChanged) => ApplyDomainEvent(finishDateChanged);

    public void ChangeTaskCompletionPercentage(TaskCompletionPercentageChanged completionPercentageChanged)
        => ApplyDomainEvent(completionPercentageChanged);

    public void ChangeTaskName(TaskNameChanged nameChanged) => ApplyDomainEvent(nameChanged);

    // TODO: Extract resource factory
    public MaterialResource CreateMaterialResource(MaterialResourceCreated resourceCreated) => (MaterialResource)ApplyDomainEvent(resourceCreated);

    public WorkingResource CreateWorkingResource(WorkingResourceCreated resourceCreated) => (WorkingResource)ApplyDomainEvent(resourceCreated);

    public void RemoveResource(ResourceRemoved resourceRemoved) => ApplyDomainEvent(resourceRemoved);

    public TaskResource AddResourceToTask(TaskResourceAdded taskResourceAdded) => (TaskResource)ApplyDomainEvent(taskResourceAdded);

    //public void ChangeTaskResourceAmount(TaskResourceAmountChanged amountChanged) => ApplyDomainEvent(amountChanged);

    public void RemoveResourceFromTask(TaskResourceRemoved taskResourceRemoved) => ApplyDomainEvent(taskResourceRemoved);

    public TaskConnection AddTaskConnection(TaskConnectionAdded connectionAdded) => (TaskConnection)ApplyDomainEvent(connectionAdded);

    public void RemoveTaskConnection(TaskConnectionRemoved connectionRemoved) => ApplyDomainEvent(connectionRemoved);

    public void Remove(ProjectRemoved projectRemoved) => ApplyDomainEvent(projectRemoved);
}

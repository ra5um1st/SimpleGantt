using System;
using SimpleGantt.Domain.ValueObjects;
using static SimpleGantt.Domain.Entities.Project;

namespace SimpleGantt.Domain.Entities.DomainTypes;

public record ConnectionType(EntityName Name) : DomainType(Name)
{
    // TODO : Extract this to different classes with interface implementation
    public static ConnectionType StartStart { get; } = new(1, nameof(StartStart), connection =>
    {
        if (connection.Child.StartDate >= connection.Parent.StartDate) return;

        var project = connection.Parent.Project;
        var difference = connection.Parent.StartDate - connection.Child.StartDate;

        if (difference > TimeSpan.Zero)
        {
            project.ChangeTaskStartDate(new TaskStartDateChanged(connection.Child.Id, connection.Parent.StartDate));
            project.ChangeTaskFinishDate(new TaskFinishDateChanged(connection.Child.Id, connection.Child.FinishDate += difference));
        }
    });

    public static ConnectionType StartFinish { get; } = new(2, nameof(StartFinish), connection =>
    {
        if (connection.Child.FinishDate >= connection.Parent.StartDate) return;

        var project = connection.Parent.Project;
        var difference = connection.Parent.StartDate - connection.Child.FinishDate;

        if (difference > TimeSpan.Zero)
        {
            project.ChangeTaskStartDate(new TaskStartDateChanged(connection.Child.Id, connection.Child.StartDate += difference));
            project.ChangeTaskFinishDate(new TaskFinishDateChanged(connection.Child.Id, connection.Parent.StartDate));
        }
    });

    public static ConnectionType FinishStart { get; } = new(3, nameof(FinishStart), connection =>
    {
        if (connection.Child.StartDate >= connection.Parent.FinishDate) return;

        var project = connection.Parent.Project;
        var difference = connection.Parent.FinishDate - connection.Child.StartDate;

        if (difference > TimeSpan.Zero)
        {
            project.ChangeTaskStartDate(new TaskStartDateChanged(connection.Child.Id, connection.Parent.FinishDate));
            project.ChangeTaskFinishDate(new TaskFinishDateChanged(connection.Child.Id, connection.Child.FinishDate += difference));
        }
    });

    public static ConnectionType FinishFinish { get; } = new(4, nameof(FinishFinish), connection =>
    {
        if (connection.Child.FinishDate <= connection.Parent.FinishDate) return;

        var project = connection.Parent.Project;
        var difference = connection.Parent.FinishDate - connection.Child.FinishDate;

        if (difference < TimeSpan.Zero)
        {
            project.ChangeTaskStartDate(new TaskStartDateChanged(connection.Child.Id, connection.Child.StartDate += difference));
            project.ChangeTaskFinishDate(new TaskFinishDateChanged(connection.Child.Id, connection.Parent.FinishDate));
        }
    });

    private readonly Action<TaskConnection> _connectionApplier;

    private ConnectionType() : this("")
    {
    }

    private ConnectionType(long id, EntityName name, Action<TaskConnection> connectionApplier) : this(name)
    {
        Id = id;
        _connectionApplier = connectionApplier ?? throw new ArgumentNullException(nameof(connectionApplier));
    }

    public static implicit operator string(ConnectionType connectionType) => connectionType.Name;

    internal void ApplyConnection(TaskConnection connection) => _connectionApplier.Invoke(connection);
}
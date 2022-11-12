using System;
using System.Data.SqlTypes;
using System.Linq;
using SimpleGantt.Domain.Events;
using SimpleGantt.Domain.Exceptions;
using SimpleGantt.Domain.ValueObjects;
using static SimpleGantt.Domain.Events.DomainEvents;
using static SimpleGantt.Domain.Queries.CommonQueries;

namespace SimpleGantt.Domain.Entities;

public partial class Project
{
    public abstract record ResourceCreated
    (
        Guid ProjectId,
        Guid ResourceId,
        EntityName Name,
        uint Count
    ) : DomainEvent
    {
        internal sealed override object Apply(object @object)
        {
            var project = (Project)@object;

            if (project.Resources.Any(item => item.Id == ResourceId)) throw new DomainExistsException(ResourceId);
            if (project.Id != ProjectId) throw new DomainEventException(nameof(ResourceCreated));

            var resource = Create(project);
            project._resources.Add(resource);

            return resource;
        }

        protected abstract Resource Create(Project project);
    }

    public record MaterialResourceCreated
    (
        Guid ProjectId,
        Guid ResourceId,
        EntityName Name,
        uint Count,
        SqlMoney Cost,
        CurrencyType CurrencyType
    ) : ResourceCreated(ProjectId, ResourceId, Name, Count)
    {
        protected override Resource Create(Project project)
        {
            return new MaterialResource(ResourceId, project, Name, Count, Cost, CurrencyType);
        }
    }

    public record WorkingResourceCreated
    (
        Guid ProjectId,
        Guid ResourceId,
        EntityName Name,
        uint Count,
        WorkScedule WorkScedule,
        Salary Salary
    ) : ResourceCreated(ProjectId, ResourceId, Name, Count)
    {
        protected override Resource Create(Project project)
        {
            return new WorkingResource(ResourceId, project, Name, Count, WorkScedule, Salary);
        }
    }

    public record ResourceRemoved
    (
        Guid ProjectId,
        Guid ResourceId
    ) : DomainEvent
    {
        internal override object Apply(object @object)
        {
            var project = (Project)@object;

            if (project.Id != ProjectId) throw new DomainEventException(nameof(ResourceRemoved));

            var resource = GetEntityById(project.Resources, ResourceId);
            project._resources.Remove(resource);

            return default!;
        }
    }

    public record ProjectCreated
    (
        Guid ProjectId,
        EntityName Name,
        DateTimeOffset ProjectCreatedAt
    ) : DomainEvent
    {
        internal override object Apply(object @object)
        {
            var project = (Project)@object;
            project.Id = ProjectId;
            project._name = Name;
            project.CreatedAt = ProjectCreatedAt;

            return project;
        }
    }

    public record ProjectRemoved
    (
        Guid RemovedEntityId
    ) : EntityRemoved(RemovedEntityId)
    {
        internal override object Apply(object @object)
        {
            var project = (Project)@object;
            project.Remove();

            return default!;
        }
    }

    public record ProjectNameChanged
    (
        Guid ProjectId,
        EntityName NewName
    ) : DomainEvent
    {
        internal override object Apply(object @object)
        {
            var project = (Project)@object;
            project._name = NewName;

            return default!;
        }
    }
}

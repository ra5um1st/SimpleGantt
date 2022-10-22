using System;
using SimpleGantt.Domain.Interfaces;
using SimpleGantt.Domain.ValueObjects;

namespace SimpleGantt.Domain.Entities.Common;

public class Resource : Entity, INamable
{
    public EntityName Name { get; private set; } = string.Empty;
    public uint Count { get; private set; }
    public Project Project { get; private set; }

    public Resource(Guid id, Project project, EntityName name, uint count) : base(id)
    {
        Project = project;
        Name = name;
        Count = count;
    }
}

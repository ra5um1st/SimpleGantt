using SimpleGantt.Domain.Interfaces;
using SimpleGantt.Domain.ValueObjects;

namespace SimpleGantt.Domain.Entities;

public class Resource : Entity, INamable
{
    public EntityName Name { get; private set; } = string.Empty;
    public uint Count { get; private set; }

    public Resource(EntityName name, uint count)
    {
        Name = name;
        Count = count;
    }
}

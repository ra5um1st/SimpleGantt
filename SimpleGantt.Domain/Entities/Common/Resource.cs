using SimpleGantt.Domain.Entities.Abstractions;

namespace SimpleGantt.Domain.Entities.Common;

public class Resource : TrackedEntity
{
    public uint Count { get; private set; }

    public Resource(string name, uint count) : base(name)
    {
        Count = count;
    }
}

using SimpleGantt.Domain.Entities.Abstractions;

namespace SimpleGantt.Domain.Entities.Common;

public class Resource : TrackedEntity
{
    public string Name { get; set; }
    public uint Count { get; set; }

    public Resource(string name, uint count)
    {
        Name = name;
        Count = count;
    }
}

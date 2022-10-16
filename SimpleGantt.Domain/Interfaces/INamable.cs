using SimpleGantt.Domain.ValueObjects;

namespace SimpleGantt.Domain.Interfaces;

public interface INamable
{
    public EntityName Name { get; }
}

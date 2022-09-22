namespace SimpleGantt.Domain.Entities.Abstractions;

public abstract class NamedEntity : Entity
{
    public string Name { get; private set; }

    public NamedEntity(string name)
    {
        Name = name;
    }
}

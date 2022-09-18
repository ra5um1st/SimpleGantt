namespace SimpleGantt.Domain.Entities.Abstractions;

public abstract class NamedEntity : Entity
{
    public string Name { get; set; }

    public NamedEntity(string name)
    {
        Name = name;
    }
}

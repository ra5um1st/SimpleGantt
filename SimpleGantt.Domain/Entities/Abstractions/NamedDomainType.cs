namespace SimpleGantt.Domain.Entities.Abstractions;

public record NamedDomainType : DomainType
{
    public string Name { get; init; }

    public NamedDomainType(long Id, string name) : base(Id)
    {
        Name = name;
    }
}

using SimpleGantt.Domain.ValueObjects;

namespace SimpleGantt.Domain.Entities;

public record UnitOfTimeType(EntityName Name) : DomainType(Name);
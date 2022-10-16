using System;
using SimpleGantt.Domain.Entities.Abstractions;
using SimpleGantt.Domain.ValueObjects;

namespace SimpleGantt.Domain.Entities;

public record UnitOfTimeType(Guid Id, EntityName Name) : DomainType(Id, Name);
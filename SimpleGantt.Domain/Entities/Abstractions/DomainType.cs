using System;
using SimpleGantt.Domain.Interfaces;
using SimpleGantt.Domain.ValueObjects;

namespace SimpleGantt.Domain.Entities;

public record DomainType(Guid Id, EntityName Name) : IEntity, INamable;

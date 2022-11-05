using System.ComponentModel.DataAnnotations;
using SimpleGantt.Domain.ValueObjects;

namespace SimpleGantt.Domain.Interfaces;

public interface INamable
{
    [Required]
    [MaxLength(EntityName.MaxLength)]
    public EntityName Name { get; }
}

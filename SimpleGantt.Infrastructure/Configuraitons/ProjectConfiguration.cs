using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SimpleGantt.Domain.Entities;
using SimpleGantt.Domain.ValueObjects;

namespace SimpleGantt.Infrastructure.Configuraitons;

public class ProjectConfiguration : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        builder.HasMany(e => e.Tasks)
            .WithOne(e => e.Project);

        builder.HasMany(e => e.Resources)
            .WithOne(e => e.Project);
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SimpleGantt.Domain.Entities;
using SimpleGantt.Domain.ValueObjects;

namespace SimpleGantt.Infrastructure.Configuraitons;

public class ProjectConfiguration : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        var nameConverter = new ValueConverter<EntityName, string>(c => c.Value, c => new EntityName(c));

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Name).HasConversion(nameConverter).IsRequired();
        builder
            .Ignore(e => e.DomainEvents)
            .Ignore(e => e.Removed)
            .Ignore(e => e.Tasks)
            .Ignore(e => e.Resources)
            .Ignore(e => e.Version);
    }
}

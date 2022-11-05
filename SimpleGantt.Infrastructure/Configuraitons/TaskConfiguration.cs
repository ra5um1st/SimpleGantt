using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SimpleGantt.Infrastructure.Configuraitons;

public class TaskConfiguration : IEntityTypeConfiguration<Domain.Entities.Task>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.Task> builder)
    {
        builder.OwnsMany(e => e.Hierarchy)
            .WithOwner(e => e.Parent);

        builder.OwnsMany(e => e.Connections)
            .WithOwner(e => e.Parent);

        builder.OwnsMany(e => e.Resources)
            .WithOwner(e => e.Task);

        builder.HasOne(e => e.Project)
            .WithMany(e => e.Tasks)
            .HasForeignKey(e => e.Project.Id)
            .IsRequired();

        builder.Property(e => e.StartDate)
            .HasConversion(typeof(long))
            .IsRequired();

        builder.Property(e => e.FinishDate)
            .HasConversion(typeof(long))
            .IsRequired();

        builder.Property(e => e.CompletionPercentage)
            .HasConversion(typeof(double))
            .IsRequired();
    }
}

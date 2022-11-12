using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SimpleGantt.Domain.Entities;
using SimpleGantt.Domain.ValueObjects;

namespace SimpleGantt.Infrastructure.Configuraitons;

public class EventStreamConfiguration : IEntityTypeConfiguration<EventInfo>
{
    public void Configure(EntityTypeBuilder<EventInfo> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.CreteadAt).IsRequired();
        builder.Property(e => e.EventType).IsRequired();
        builder.Property(e => e.Version).IsRequired();
        builder.Property(e => e.AggregateId).IsRequired();
        builder.Property(e => e.AggregateType).IsRequired();
        builder.Property(e => e.Data).HasMaxLength(10_000).IsRequired();
    }
}

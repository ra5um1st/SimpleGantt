using Microsoft.EntityFrameworkCore;
using SimpleGantt.Domain.Entities;
using SimpleGantt.Domain.Entities.DomainTypes;
using SimpleGantt.Infrastructure.Configuraitons;
using Task = SimpleGantt.Domain.Entities.Task;

namespace SimpleGantt.Infrastructure.EntityFramework.Contexts;

public class ProjectDbContext : DbContext
{
    public DbSet<Project> Projects => Set<Project>();
    public DbSet<Task> Tasks => Set<Task>();
    public DbSet<EventInfo> EventStream => base.Set<EventInfo>();

    public ProjectDbContext()
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        modelBuilder.ApplyConfiguration(new ProjectConfiguration());
        modelBuilder.ApplyConfiguration(new EventStreamConfiguration());
        modelBuilder.Ignore<ConnectionType>();
        modelBuilder.Ignore<Task>();
        modelBuilder.Ignore<Resource>();
        base.OnModelCreating(modelBuilder);
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseInMemoryDatabase("TestDatabase");
    }
}

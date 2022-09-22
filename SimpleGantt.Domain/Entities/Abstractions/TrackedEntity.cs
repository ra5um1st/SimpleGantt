using System;

namespace SimpleGantt.Domain.Entities.Abstractions;

public abstract class TrackedEntity : NamedEntity
{
    // TODO : Add users and authentication to domain model

    //public User Creator { get; set; }
    //public User Updater { get; set; }

    public DateTimeOffset Created { get; private set; }
    public DateTimeOffset Updated { get; private set; }

    public TrackedEntity(string name, DateTimeOffset created, DateTimeOffset updated) : base(name)
    {
        Created = created;
        Updated = updated;
    }

    public TrackedEntity(string name) : base(name)
    {
        Created = DateTimeOffset.Now;
        Updated = DateTimeOffset.Now;
    }
}

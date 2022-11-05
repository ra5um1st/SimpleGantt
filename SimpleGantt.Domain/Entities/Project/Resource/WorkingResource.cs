using System;
using SimpleGantt.Domain.ValueObjects;

namespace SimpleGantt.Domain.Entities;

public class WorkingResource : Resource
{
    public WorkScedule WorkScedule { get; private set; }
    public Salary Salary { get; set; }

    public WorkingResource(Guid id, Project project, EntityName name, uint count, WorkScedule workScedule, Salary salary) : base(id, project, name, count)
    {
        WorkScedule = workScedule;
        Salary = salary;
    }
}

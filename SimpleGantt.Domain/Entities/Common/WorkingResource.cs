namespace SimpleGantt.Domain.Entities.Common;

public class WorkingResource : Resource
{
    public WorkScedule WorkScedule { get; set; }
    public Salary Salary { get; set; }

    public WorkingResource(string name, uint count, WorkScedule workScedule, Salary salary) : base(name, count)
    {
        WorkScedule = workScedule;
        Salary = salary;
    }
}

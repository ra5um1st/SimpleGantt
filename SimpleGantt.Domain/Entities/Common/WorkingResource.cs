namespace SimpleGantt.Domain.Entities;

public class WorkingResource : Resource
{
    public WorkScedule WorkScedule { get; private set; }
    public Salary Salary { get; set; }

    public WorkingResource(string name, uint count, WorkScedule workScedule, Salary salary) : base(name, count)
    {
        WorkScedule = workScedule;
        Salary = salary;
    }
}

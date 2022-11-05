namespace SimpleGantt.Domain.Tests;

public class ResoruceTests
{
    private static readonly DateTimeOffset Now = DateTimeOffset.Now;

    [Fact]
    public void AddMaterialResource_CorrectResource_ShouldAdd()
    {
        var project = Project.Create(new ProjectCreated(Guid.NewGuid(), "test project", DateTimeOffset.Now));
        var materialResource = project.CreateMaterialResource(new MaterialResourceCreated(project.Id, Guid.NewGuid(), "material 1", 5, 100, CurrencyType.RUB));

        Assert.Contains(materialResource, project.Resources);
    }

    [Fact]
    public void AddWorkingResource_CorrectResource_ShouldAdd()
    {
        var project = Project.Create(new ProjectCreated(Guid.NewGuid(), "test project", DateTimeOffset.Now));
        var workWeekScedule = new WorkWeekScedule(Guid.NewGuid(), "default work week schedule", Now);
        var workSchedule = new WorkScedule(Guid.NewGuid(), "default schedule", workWeekScedule);
        var salary = new Salary(Guid.NewGuid(), 1000, SalaryType.RubPerMonth);
        var workingResource = project.CreateWorkingResource(new WorkingResourceCreated(project.Id, Guid.NewGuid(), "worker 1", 1, workSchedule, salary));

        Assert.Contains(workingResource, project.Resources);
    }

    [Fact]
    public void RemoveMaterialResource_CorrectResource_ShouldRemove()
    {
        var project = Project.Create(new ProjectCreated(Guid.NewGuid(), "test project", DateTimeOffset.Now));
        var workWeekScedule = new WorkWeekScedule(Guid.NewGuid(), "default work week schedule", Now);
        var workSchedule = new WorkScedule(Guid.NewGuid(), "default schedule", workWeekScedule);
        var salary = new Salary(Guid.NewGuid(), 1000, SalaryType.RubPerMonth);
        var workingResource = project.CreateWorkingResource(new WorkingResourceCreated(project.Id, Guid.NewGuid(), "worker 1", 1, workSchedule, salary));
        project.RemoveResource(new ResourceRemoved(project.Id, workingResource.Id));

        Assert.DoesNotContain(workingResource, project.Resources);
    }

}

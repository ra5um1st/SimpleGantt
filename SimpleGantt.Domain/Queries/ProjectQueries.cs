using System;
using System.Linq;
using SimpleGantt.Domain.Entities;

namespace SimpleGantt.Domain.Queries;

public static class ProjectQueries
{
    public static double GetUsedResourcesAmountById(this Project project, Guid taskResourceId) =>
        project.Tasks.Sum(item => item.Resources.First(taskResource => taskResource.Id == taskResourceId).Count);
}

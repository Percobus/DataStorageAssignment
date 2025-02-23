using Business.Dtos;
using Data.Entities;

namespace Business.Factories;

public class ProjectFactory
{
    public static ProjectDataTransfer CreateProjectDataTransfer(ProjectEntity projectEntity)
    {
        var projectDataTransfer = new ProjectDataTransfer
        {
            ProjectNumber = projectEntity.ProjectNumber,
            Name = projectEntity.Title,
            StartDate = projectEntity.StartDate,
            EndDate = projectEntity.EndDate,
            CustomerId = projectEntity.CustomerId,
            CustomerName = projectEntity.Customer.CustomerName,
            StatusTypeName = projectEntity.Status.StatusName
        };

        return projectDataTransfer; 
    }

    public static ProjectEntity CreateProjectEntity(ProjectDataTransfer projectDataTransfer)
    {
        return new ProjectEntity
        {
            Title = projectDataTransfer.Name,
            StartDate = projectDataTransfer.StartDate,
            EndDate = projectDataTransfer.EndDate,
            CustomerId = projectDataTransfer.CustomerId,
            StatusId = 1,
            ProductId = 1
        };
    }
}

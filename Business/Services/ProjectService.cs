using Business.Dtos;
using Business.Interfaces;
using Business.Models;

namespace Business.Services;

public class ProjectService : IProjectService
{
    private readonly IProjectRepository _projectRepository;

    public ProjectService(IProjectRepository projectRepository)
    {
        _projectRepository = projectRepository;
    }

    public async Task<IEnumerable<ProjectModel>> GetProjectsAsync()
    {
        var projects = await _projectRepository.GetAllProjectsAsync();
        return projects.Select(p => new ProjectModel
        {
            ProjectNumber = p.ProjectNumber,
            Name = p.Name,
            StartDate = p.StartDate,
            EndDate = p.EndDate,
            CustomerId = p.CustomerId,
            CustomerName = p.CustomerName,
            StatusTypeId = p.StatusTypeId,
            StatusTypeName = p.StatusTypeName,
            PricePerHour = p.PricePerHour,
            TotalHours = p.TotalHours
        });
    }

    public async Task<ProjectModel?> GetProjectByIdAsync(int id)
    {
        var project = await _projectRepository.GetProjectByIdAsync(id);
        if (project == null)
            return null;

        return new ProjectModel
        {
            ProjectNumber = project.ProjectNumber,
            Name = project.Name,
            StartDate = project.StartDate,
            EndDate = project.EndDate,
            CustomerId = project.CustomerId,
            CustomerName = project.CustomerName,
            StatusTypeId = project.StatusTypeId,
            StatusTypeName = project.StatusTypeName,
            PricePerHour = project.PricePerHour,
            TotalHours = project.TotalHours
        };
    }

    public async Task<ProjectModel> AddProjectAsync(ProjectModel projectModel, int userId)
    {
        var projectDto = new ProjectDataTransfer
        {
            ProjectNumber = projectModel.ProjectNumber,
            Name = projectModel.Name,
            StartDate = projectModel.StartDate,
            EndDate = projectModel.EndDate,
            CustomerId = projectModel.CustomerId,
            CustomerName = projectModel.CustomerName,
            StatusTypeId = projectModel.StatusTypeId,
            PricePerHour = projectModel.PricePerHour,
            TotalHours = projectModel.TotalHours,
            UserId = userId
        };

        var createdProject = await _projectRepository.AddProjectAsync(projectDto);
        return new ProjectModel
        {
            ProjectNumber = createdProject.ProjectNumber,
            Name = createdProject.Name,
            StartDate = createdProject.StartDate,
            EndDate = createdProject.EndDate,
            CustomerId = createdProject.CustomerId,
            CustomerName = createdProject.CustomerName,
            StatusTypeId = createdProject.StatusTypeId,
            StatusTypeName = createdProject.StatusTypeName,
            PricePerHour = createdProject.PricePerHour,
            TotalHours = createdProject.TotalHours
        };
    }

    public async Task<ProjectModel?> UpdateProjectAsync(int id, ProjectModel projectModel)
    {
        var existingProject = await _projectRepository.GetProjectByIdAsync(id);
        if (existingProject == null)
            return null;

        var projectDto = new ProjectDataTransfer
        {
            Name = string.IsNullOrEmpty(projectModel.Name) ? existingProject.Name : projectModel.Name,
            StartDate = projectModel.StartDate == default ? existingProject.StartDate : projectModel.StartDate,
            EndDate = projectModel.EndDate == default ? existingProject.EndDate : projectModel.EndDate,
            CustomerId = projectModel.CustomerId == 0 ? existingProject.CustomerId : projectModel.CustomerId,
            CustomerName = string.IsNullOrEmpty(projectModel.CustomerName) ? existingProject.CustomerName : projectModel.CustomerName,
            StatusTypeId = projectModel.StatusTypeId == 0 ? existingProject.StatusTypeId : projectModel.StatusTypeId,
            PricePerHour = projectModel.PricePerHour == 0 ? existingProject.PricePerHour : projectModel.PricePerHour,
            TotalHours = projectModel.TotalHours == 0 ? existingProject.TotalHours : projectModel.TotalHours
        };

        var updatedProject = await _projectRepository.UpdateProjectAsync(id, projectDto);
        if (updatedProject == null)
            return null;

        return new ProjectModel
        {
            ProjectNumber = updatedProject.ProjectNumber,
            Name = updatedProject.Name,
            StartDate = updatedProject.StartDate,
            EndDate = updatedProject.EndDate,
            CustomerId = updatedProject.CustomerId,
            CustomerName = updatedProject.CustomerName,
            StatusTypeId = updatedProject.StatusTypeId,
            StatusTypeName = updatedProject.StatusTypeName,
            PricePerHour = updatedProject.PricePerHour,
            TotalHours = updatedProject.TotalHours
        };
    }

    public async Task<bool> DeleteProjectAsync(int id)
    {
        return await _projectRepository.DeleteProjectAsync(id);
    }

    public async Task<ProjectModel> AddProjectWithCustomerAsync(string projectName, DateTime startDate, DateTime endDate, string customerName, string productName, int statusTypeId, decimal pricePerHour, int totalHours, int userId)
    {
        var createdProject = await _projectRepository.AddProjectWithCustomerAsync(projectName, startDate, endDate, customerName, productName, statusTypeId, pricePerHour, totalHours, userId);

        return new ProjectModel
        {
            ProjectNumber = createdProject.ProjectNumber,
            Name = createdProject.Name,
            StartDate = createdProject.StartDate,
            EndDate = createdProject.EndDate,
            CustomerId = createdProject.CustomerId,
            CustomerName = createdProject.CustomerName,
            StatusTypeId = createdProject.StatusTypeId,
            StatusTypeName = createdProject.StatusTypeName,
            PricePerHour = createdProject.PricePerHour,
            TotalHours = createdProject.TotalHours
        };
    }
}

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
            Price = p.Price,
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
            Price = project.Price,
        };
    }

    public async Task<ProjectModel> AddProjectAsync(ProjectModel projectModel)
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
            Price = projectModel.Price
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
            Price = createdProject.Price,
        };
    }

    public async Task<ProjectModel?> UpdateProjectAsync(int id, ProjectModel projectModel)
    {
        var projectDto = new ProjectDataTransfer
        {
            Name = projectModel.Name,
            StartDate = projectModel.StartDate,
            EndDate = projectModel.EndDate,
            CustomerId = projectModel.CustomerId,
            CustomerName = projectModel.CustomerName,
            StatusTypeId = projectModel.StatusTypeId,
            Price = projectModel.Price
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
            Price = updatedProject.Price,
        };
    }

    public async Task<bool> DeleteProjectAsync(int id)
    {
        return await _projectRepository.DeleteProjectAsync(id);
    }
}

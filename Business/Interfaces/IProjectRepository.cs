
using Business.Dtos;

namespace Business.Interfaces;

public interface IProjectRepository
{
    Task<ProjectDataTransfer> AddProjectAsync(ProjectDataTransfer project);
    Task<IEnumerable<ProjectDataTransfer>> GetAllProjectsAsync();
    Task<ProjectDataTransfer?> GetProjectByIdAsync(int id);
    Task<ProjectDataTransfer> UpdateProjectAsync(int id, ProjectDataTransfer project);
    Task<bool> DeleteProjectAsync(int id);
}

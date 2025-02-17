using Business.Models;

namespace Business.Interfaces;

public interface IProjectService
{
    Task<IEnumerable<ProjectModel>> GetProjectsAsync();
    Task<ProjectModel?> GetProjectByIdAsync(int id);
    Task<ProjectModel> AddProjectAsync(ProjectModel projectModel);
    Task<ProjectModel?> UpdateProjectAsync(int id, ProjectModel projectModel);
    Task<bool> DeleteProjectAsync(int id);
}

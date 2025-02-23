using Business.Models;

namespace Business.Interfaces;

public interface IProjectService
{
    Task<IEnumerable<ProjectModel>> GetProjectsAsync();
    Task<ProjectModel?> GetProjectByIdAsync(int id);
    Task<ProjectModel> AddProjectAsync(ProjectModel projectModel, int userId);
    Task<ProjectModel?> UpdateProjectAsync(int id, ProjectModel projectModel);
    Task<ProjectModel> AddProjectWithCustomerAsync(string projectName, DateTime startDate, DateTime endDate, string customerName, string productName, int statusId, decimal pricePerHour, int totalHours, int userId);
    Task<bool> DeleteProjectAsync(int id);
}

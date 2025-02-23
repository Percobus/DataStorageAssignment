using Business.Dtos;

namespace Business.Interfaces;

public interface IProjectRepository
{
    Task<ProjectDataTransfer> AddProjectAsync(ProjectDataTransfer project);
    Task<ProjectDataTransfer> AddProjectWithCustomerAsync(string projectName, DateTime startDate, DateTime endDate, string customerName, string productName, int statusTypeId, decimal pricePerHour, int totalHours, int userId);
    Task<IEnumerable<ProjectDataTransfer>> GetAllProjectsAsync();
    Task<ProjectDataTransfer?> GetProjectByIdAsync(int id);
    Task<ProjectDataTransfer> UpdateProjectAsync(int id, ProjectDataTransfer project);
    Task<bool> DeleteProjectAsync(int id);
}

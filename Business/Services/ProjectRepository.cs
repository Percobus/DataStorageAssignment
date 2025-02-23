using Business.Dtos;
using Business.Interfaces;
using Data.Contexts;
using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Business.Services;

public class ProjectRepository : IProjectRepository
{
    private readonly DataContext _context;

    public ProjectRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<ProjectDataTransfer> AddProjectAsync(ProjectDataTransfer project)
    {
        var projectEntity = new ProjectEntity
        {
            Title = project.Name,
            StartDate = project.StartDate,
            EndDate = project.EndDate,
            CustomerId = project.CustomerId,
            StatusId = project.StatusTypeId,
            ProductId = null,
            UserId = project.UserId,
            PricePerHour = project.PricePerHour,
            TotalHours = project.TotalHours
        };

        try
        {
            _context.Projects.Add(projectEntity);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }

        return new ProjectDataTransfer
        {
            ProjectNumber = projectEntity.ProjectNumber,
            Name = projectEntity.Title,
            StartDate = projectEntity.StartDate,
            EndDate = projectEntity.EndDate,
            CustomerId = projectEntity.CustomerId,
            CustomerName = project.CustomerName,
            StatusTypeId = projectEntity.StatusId,
            StatusTypeName = project.StatusTypeName,
            PricePerHour = projectEntity.PricePerHour,
            TotalHours = projectEntity.TotalHours
        };
    }

    public async Task<ProjectDataTransfer> AddProjectWithCustomerAsync(string projectName, DateTime startDate, DateTime endDate, string customerName, string productName, int statusId, decimal pricePerHour, int totalHours, int userId)
    {
        // Skapa en ny kund
        var customerEntity = new CustomerEntity { CustomerName = customerName };
        _context.Customers.Add(customerEntity);
        await _context.SaveChangesAsync();

        // Skapa en ny produkt
        var productEntity = new ProductEntity { ProductName = productName };
        _context.Products.Add(productEntity);
        await _context.SaveChangesAsync();

        // Skapa ett nytt projekt
        var projectEntity = new ProjectEntity
        {
            Title = projectName,
            StartDate = startDate,
            EndDate = endDate,
            CustomerId = customerEntity.Id,
            ProductId = productEntity.Id,
            StatusId = statusId,
            UserId = userId,
            PricePerHour = pricePerHour,
            TotalHours = totalHours
        };

        _context.Projects.Add(projectEntity);
        await _context.SaveChangesAsync();

        // Returdata transfer för projektet
        return new ProjectDataTransfer
        {
            ProjectNumber = projectEntity.ProjectNumber,
            Name = projectEntity.Title,
            StartDate = projectEntity.StartDate,
            EndDate = projectEntity.EndDate,
            CustomerId = projectEntity.CustomerId,
            CustomerName = customerEntity.CustomerName,
            StatusTypeId = projectEntity.StatusId,
            PricePerHour = projectEntity.PricePerHour,
            TotalHours = projectEntity.TotalHours
        };
    }

    public async Task<IEnumerable<ProjectDataTransfer>> GetAllProjectsAsync()
    {
        return await _context.Projects.Include(p => p.Product)
            .Select(p => new ProjectDataTransfer
            {
                ProjectNumber = p.ProjectNumber,
                Name = p.Title,
                StartDate = p.StartDate,
                EndDate = p.EndDate,
                CustomerId = p.CustomerId,
                CustomerName = p.Customer.CustomerName,
                StatusTypeId = p.StatusId,
                StatusTypeName = p.Status.StatusName,
                PricePerHour = p.PricePerHour,
                TotalHours = p.TotalHours
            }).ToListAsync();
    }

    public async Task<ProjectDataTransfer?> GetProjectByIdAsync(int projectNumber)
    {
        var project = await _context.Projects
            .Include(p => p.Customer)
            .Include(p => p.Status)
            .Include(p => p.Product)
            .FirstOrDefaultAsync(p => p.ProjectNumber == projectNumber);

        if (project == null)
            return null;

        return new ProjectDataTransfer
        {
            ProjectNumber = project.ProjectNumber,
            Name = project.Title,
            StartDate = project.StartDate,
            EndDate = project.EndDate,
            CustomerId = project.CustomerId,
            CustomerName = project.Customer.CustomerName,
            StatusTypeId = project.StatusId,
            StatusTypeName = project.Status.StatusName,
            PricePerHour = project.PricePerHour,
            TotalHours = project.TotalHours
        };
    }

    public async Task<ProjectDataTransfer> UpdateProjectAsync(int projectNumber, ProjectDataTransfer project)
    {
        var existingProject = await _context.Projects.FirstOrDefaultAsync(p => p.ProjectNumber == projectNumber);
        if (existingProject == null)
            throw new InvalidOperationException("Project not found");

        existingProject.Title = project.Name;
        existingProject.StartDate = project.StartDate;
        existingProject.EndDate = project.EndDate;
        existingProject.StatusId = project.StatusTypeId;
        existingProject.PricePerHour = project.PricePerHour;
        existingProject.TotalHours = project.TotalHours;

        var product = await _context.Products.FindAsync(existingProject.ProductId);
        await _context.SaveChangesAsync();

        return project;
    }

    public async Task<bool> DeleteProjectAsync(int projectNumber)
    {
        var project = await _context.Projects.FirstOrDefaultAsync(p => p.ProjectNumber == projectNumber);
        if (project == null)
            return false;

        _context.Projects.Remove(project);
        await _context.SaveChangesAsync();

        return true;
    }
}

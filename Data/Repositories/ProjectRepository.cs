using Data.Contexts;
using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public class ProjectRepository
{
    private readonly DataContext _context;

    public ProjectRepository(DataContext context)
    {
        _context = context;
    }

    // CREATE
    public async Task<ProjectEntity> AddProjectAsync(ProjectEntity project)
    {
        await _context.Projects.AddAsync(project);
        await _context.SaveChangesAsync();
        return project;
    }

    // READ - ALL
    public async Task<IEnumerable<ProjectEntity>> GetAllProjectsAsync()
    {
        return await _context.Projects
            .Include(p => p.Status)  // Inkludera Status
            .Include(p => p.Customer) // Inkludera Customer
            .Include(p => p.Product)  // Inkludera Product
            .ToListAsync();
    }

    // READ - BY ID
    public async Task<ProjectEntity?> GetProjectByIdAsync(int id)
    {
        return await _context.Projects
            .Include(p => p.Status)
            .Include(p => p.Customer)
            .Include(p => p.Product)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    // UPDATE
    public async Task<ProjectEntity> UpdateProjectAsync(int id, ProjectEntity updatedProject)
    {
        var existingProject = await _context.Projects.FindAsync(id);
        if (existingProject == null) return null;

        existingProject.Title = updatedProject.Title;
        existingProject.Description = updatedProject.Description;
        existingProject.StartDate = updatedProject.StartDate;
        existingProject.EndDate = updatedProject.EndDate;
        existingProject.CustomerId = updatedProject.CustomerId;
        existingProject.StatusId = updatedProject.StatusId;
        existingProject.UserId = updatedProject.UserId;
        existingProject.ProductId = updatedProject.ProductId;

        _context.Projects.Update(existingProject);
        await _context.SaveChangesAsync();
        return existingProject;
    }

    // DELETE
    public async Task<bool> DeleteProjectAsync(int id)
    {
        var project = await _context.Projects.FindAsync(id);
        if (project == null)
            return false;

        _context.Projects.Remove(project);
        await _context.SaveChangesAsync();
        return true;
    }
}
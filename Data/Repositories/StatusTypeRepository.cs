using Data.Contexts;
using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public class StatusTypeRepository(DataContext context)
{
    private readonly DataContext _context = context;


    // CREATE
    public async Task<StatusTypeEntity> AddStatusTypeAsync(StatusTypeEntity statusType)
    {
        await _context.StatusTypes.AddAsync(statusType);
        await _context.SaveChangesAsync();
        return statusType;
    }

    // READ - ALL
    public async Task<IEnumerable<StatusTypeEntity>> GetAllStatusTypesAsync()
    {
        return await _context.StatusTypes.ToListAsync();
    }

    // READ - ID
    public async Task<StatusTypeEntity?> StatusTypeEntityAsync(int id, StatusTypeEntity updatedStatusType)
    {
        var statusType = await _context.StatusTypes.FindAsync(id);
        if (statusType == null)
            return null;

        statusType.StatusName = updatedStatusType.StatusName;

        _context.StatusTypes.Update(statusType);
        await _context.SaveChangesAsync();
        return statusType;
    }

    // DELETE
    public async Task<bool> DeleteStatusTypeAsync(int id)
    {
        var statusType = await _context.StatusTypes.FindAsync(id);
        if (statusType == null)
            return false;

        _context.StatusTypes.Remove(statusType);
        await _context.SaveChangesAsync();
        return true;
    }
}
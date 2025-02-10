using Data.Contexts;
using Data.Entities;

namespace Data.Repositories;

public class UserRepository(DataContext context)
{
    private readonly DataContext _context = context;


    // CREATE
    public async Task<UserEntity> AddUserAsync(UserEntity user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
        return user;
    }

    // READ
    public async Task<UserEntity?> GetUserByIdAsync(int id)
    {
        return await _context.Users.FindAsync(id);
    }

    // UPDATE
    public async Task<UserEntity?> UpdateUserAsync(int id, UserEntity updatedUser)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
            return null;

        user.FirstName = updatedUser.FirstName;
        user.LastName = updatedUser.LastName;
        user.Email = updatedUser.Email;

        _context.Users.Update(user);
        await _context.SaveChangesAsync();
        return user;
    }

    // DELETE
    public async Task<bool> DeleteAsync(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if ( user == null)
            return false;

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        return true;
    }
}
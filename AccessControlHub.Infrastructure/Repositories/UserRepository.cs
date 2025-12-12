using AccessControlHub.Domain.Entities;
using AccessControlHub.Domain.Repositories;
using AccessControlHub.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AccessControlHub.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AccessControlHubDbContext _dbContext;

    public UserRepository(AccessControlHubDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<User>> GetAllAsync()
    {
        return await _dbContext.Users.ToListAsync();
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        return await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<User> AddAsync(User user)
    {
        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();
        return user;
    }

    public async Task<User?> UpdateAsync(User user)
    {
        var existingUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == user.Id);

        if (existingUser is null)
        {
            return null;
        }

        existingUser.Name = user.Name;
        existingUser.Email = user.Email;

        await _dbContext.SaveChangesAsync();
        return existingUser;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);

        if (user is null)
        {
            return false;
        }

        _dbContext.Users.Remove(user);
        await _dbContext.SaveChangesAsync();

        return true;
    }
}
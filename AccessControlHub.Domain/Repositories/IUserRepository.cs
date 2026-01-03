using AccessControlHub.Domain.Entities;

namespace AccessControlHub.Domain.Repositories;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(int id);
    Task<IReadOnlyList<User>> GetAllAsync();
    Task AddAsync(User user);
    Task<User?> GetByEmailAsync(string email);
}
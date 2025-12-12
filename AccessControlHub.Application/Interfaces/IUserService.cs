using AccessControlHub.Application.Dtos.Users;

namespace AccessControlHub.Application.Interfaces;

public interface IUserService
{
    Task<List<UserResponseDto>> GetAllAsync();
    Task<UserResponseDto?> GetByIdAsync(int id);
    Task<UserResponseDto> CreateAsync(CreateUserDto input);
    Task<UserResponseDto?> UpdateAsync(int id, UpdateUserDto input);
    Task<bool> DeleteAsync(int id);
}
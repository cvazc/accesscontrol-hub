using AccessControlHub.Application.Dtos.Users;
using AccessControlHub.Application.Interfaces;
using AccessControlHub.Domain.Entities;
using AccessControlHub.Domain.Repositories;
using System.Security.Cryptography;
using System.Text;

namespace AccessControlHub.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<List<UserResponseDto>> GetAllAsync()
    {
        var users = await _userRepository.GetAllAsync();

        return users.Select(u => new UserResponseDto
        {
            Id = u.Id,
            Name = u.Name,
            Email = u.Email
        }).ToList();
    }

    public async Task<UserResponseDto?> GetByIdAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);

        if (user is null)
        {
            return null;
        }

        return new UserResponseDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email
        };
    }

    public async Task<UserResponseDto> CreateAsync(CreateUserDto input)
    {
        if (string.IsNullOrWhiteSpace(input.Name))
            throw new ArgumentException("Name is required");

        if (string.IsNullOrWhiteSpace(input.Email))
            throw new ArgumentException("Email is required");

        if (string.IsNullOrWhiteSpace(input.Password))
            throw new ArgumentException("Password is required");

        var passwordHash = HashPassword(input.Password);

        var newUser = new User
        {
            Name = input.Name,
            Email = input.Email,
            PasswordHash = passwordHash
        };

        var createdUser = await _userRepository.AddAsync(newUser);

        return new UserResponseDto
        {
            Id = createdUser.Id,
            Name = createdUser.Name,
            Email = createdUser.Email
        };
    }

    public async Task<UserResponseDto?> UpdateAsync(int id, UpdateUserDto input)
    {
        var existingUser = await _userRepository.GetByIdAsync(id);

        if (existingUser is null)
        {
            return null;
        }

        existingUser.Name = input.Name;
        existingUser.Email = input.Email;

        var updatedUser = await _userRepository.UpdateAsync(existingUser);

        if (updatedUser is null)
        {
            return null;
        }

        return new UserResponseDto
        {
            Id = updatedUser.Id,
            Name = updatedUser.Name,
            Email = updatedUser.Email
        };
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _userRepository.DeleteAsync(id);
    }

    private string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();

        var bytes = Encoding.UTF8.GetBytes(password);
        var hashBytes = sha256.ComputeHash(bytes);

        var stringBuilder = new StringBuilder();
        foreach (var b in hashBytes)
        {
            stringBuilder.Append(b.ToString("x2"));
        }

        return stringBuilder.ToString();
    }
}
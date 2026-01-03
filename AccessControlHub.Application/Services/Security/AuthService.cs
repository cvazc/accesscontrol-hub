using System.Security.Claims;
using AccessControlHub.Application.Dtos.Auth;
using AccessControlHub.Application.Interfaces.Security;
using AccessControlHub.Domain.Repositories;

namespace AccessControlHub.Application.Services.Security;

public class AuthService : IAuthService
{
    private readonly ITokenService _tokenService;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUserRepository _userRepository;

    public AuthService(
        ITokenService tokenService,
        IPasswordHasher passwordHasher,
        IUserRepository userRepository)
    {
        _tokenService = tokenService;
        _passwordHasher = passwordHasher;
        _userRepository = userRepository;
    }

    public async Task<LoginResponseDto> LoginAsync(LoginRequestDto request)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email);

        if (user is null)
        {
            throw new UnauthorizedAccessException("Invalid credentials");
        }

        var validPassword = _passwordHasher.VerifyPassword(
            request.Password,
            user.PasswordHash
        );

        if (!validPassword)
        {
            throw new UnauthorizedAccessException("Invalid credentials");
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role),
            new Claim("permission", "users.read")
        };

        var token = _tokenService.GenerateToken(claims);

        return new LoginResponseDto
        {
            AccessToken = token
        };
    }
}

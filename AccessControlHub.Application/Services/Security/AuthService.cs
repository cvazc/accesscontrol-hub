using System.Security.Claims;
using AccessControlHub.Application.Dtos.Auth;
using AccessControlHub.Application.Interfaces.Security;

namespace AccessControlHub.Application.Services.Security;

public class AuthService : IAuthService
{
    private readonly ITokenService _tokenService;

    public AuthService(ITokenService tokenService)
    {
        _tokenService = tokenService;
    }

    public LoginResponseDto Login(LoginRequestDto request)
    {
        if (request.Email != "admin@accesshub.com" || request.Password != "Password123!")
        {
            throw new UnauthorizedAccessException("Invalid credentials");
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, "1"),
            new Claim(ClaimTypes.Email, request.Email),
            new Claim(ClaimTypes.Role, "Admin"),
            new Claim("permission", "users.read")
        };

        var token = _tokenService.GenerateToken(claims);

        return new LoginResponseDto
        {
            AccessToken = token
        };
    }
}

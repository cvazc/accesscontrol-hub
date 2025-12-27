using System.Security.Claims;
using AccessControlHub.Application.Dtos.Auth;
using AccessControlHub.Application.Interfaces.Security;

namespace AccessControlHub.Application.Services.Security;

public class AuthService : IAuthService
{
    private readonly ITokenService _tokenService;
    private readonly IPasswordHasher _passwordHasher;
    private const string MockEmail = "admin@accesshub.com";
    private const string MockPasswordHash = "$2a$11$t61n3cXxqA1z4ILPj0/Y8eS0tKKmEbxSjFKR4m.kL7oA5B6IkEKiO";

    public AuthService(ITokenService tokenService, IPasswordHasher passwordHasher)
    {
        _tokenService = tokenService;
        _passwordHasher = passwordHasher;
    }

    public LoginResponseDto Login(LoginRequestDto request)
    {
        if (!string.Equals(request.Email, MockEmail, StringComparison.OrdinalIgnoreCase))
        {
            throw new UnauthorizedAccessException("Invalid credentials");
        }

        var validPassword = _passwordHasher.VerifyPassword(request.Password, MockPasswordHash);

        if (!validPassword)
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

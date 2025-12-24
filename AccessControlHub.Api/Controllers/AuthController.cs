using System.Security.Claims;
using AccessControlHub.Application.Dtos.Auth;
using AccessControlHub.Application.Interfaces.Security;
using Microsoft.AspNetCore.Mvc;

namespace AccessControlHub.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly ITokenService _tokenService;

    public AuthController(ITokenService tokenService)
    {
        _tokenService = tokenService;
    }

    [HttpPost("login")]
    public IActionResult Login(LoginRequestDto request)
    {
        if (request.Email != "admin@accesshub.com" || request.Password != "Password123!")
        {
            return Unauthorized("Invalid credentials");
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, "1"),
            new Claim(ClaimTypes.Email, request.Email),
            new Claim(ClaimTypes.Role, "Admin"),
            new Claim("permission", "users.read")
        };

        var token = _tokenService.GenerateToken(claims);

        return Ok(new LoginResponseDto
        {
            AccessToken = token
        });
    }
}
using System.Security.Claims;
using AccessControlHub.Application.Dtos.Auth;
using AccessControlHub.Application.Interfaces.Security;
using Microsoft.AspNetCore.Mvc;

namespace AccessControlHub.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public IActionResult Login(LoginRequestDto request)
    {
        var response = _authService.Login(request);
        return Ok(response);
    }
}
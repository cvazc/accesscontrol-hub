using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AccessControlHub.Api.Controllers;

[ApiController]
[Route("api/test")]
public class TestController : ControllerBase
{
    [HttpGet("protected")]
    [Authorize]
    public IActionResult ProtectedEndpoint()
    {
        return Ok("You are authenticated!");
    }

    [HttpGet("admin-only")]
    [Authorize(Roles = "Admin")]
    public IActionResult AdminOnly()
    {
        return Ok("YOu are authorized as Admin");
    }

    [HttpGet("policy-admin")]
    [Authorize(Policy = "AdminOnly")]
    public IActionResult PolicyAdmin()
    {
        return Ok("Policy AdminOnly passed!");
    }

    [HttpGet("policy-users-read")]
    [Authorize(Policy = "CanReadUsers")]
    public IActionResult PolicyUsersRead()
    {
        return Ok("Policy CanReadUsers passed!");
    }
}

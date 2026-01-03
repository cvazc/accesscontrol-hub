using AccessControlHub.Application.Dtos.Auth;

namespace AccessControlHub.Application.Interfaces.Security;

public interface IAuthService
{
    Task<LoginResponseDto> LoginAsync(LoginRequestDto request);
}

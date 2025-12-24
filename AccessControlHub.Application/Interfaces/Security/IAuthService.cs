using AccessControlHub.Application.Dtos.Auth;

namespace AccessControlHub.Application.Interfaces.Security;

public interface IAuthService
{
    LoginResponseDto Login(LoginRequestDto request);
}

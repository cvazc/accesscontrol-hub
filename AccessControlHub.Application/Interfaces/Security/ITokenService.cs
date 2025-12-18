using System.Security.Claims;

namespace AccessControlHub.Application.Interfaces.Security;

public interface ITokenService
{
    string GenerateToken(IEnumerable<Claim> claims);
}

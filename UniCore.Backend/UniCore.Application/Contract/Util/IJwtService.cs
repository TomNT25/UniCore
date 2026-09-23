using System.Security.Claims;
using UniCore.Application.DTO.Entity;

namespace UniCore.Application.Contract.Util
{
    public interface IJwtService
    {
        string GenerateAccessToken(UserDTO user);
        string GenerateRefreshToken();
        ClaimsPrincipal? GetPrincipalFromToken(string token);
        bool ValidateToken(string token);
    }
}

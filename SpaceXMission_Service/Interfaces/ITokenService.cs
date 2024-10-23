using SpaceXMission_Domain.Dtos;
using SpaceXMission_Shared.Helpers.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace SpaceXMission_Service.Interfaces
{
    public interface ITokenService
    {
        JwtSecurityToken GetToken(IEnumerable<Claim> authClaims);
        string GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
        Task<ApiResponse<AuthenticatedResponse>> Refresh(TokenModel tokenModel);
    }
}

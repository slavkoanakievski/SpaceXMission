using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace SpaceXMission_Service.Interfaces
{
    public interface ITokenService
    {
        JwtSecurityToken GetToken(IEnumerable<Claim> authClaims);
    }
}

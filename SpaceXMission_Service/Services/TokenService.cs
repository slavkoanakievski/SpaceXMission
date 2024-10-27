using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SpaceXMission.Entities;
using SpaceXMission_Domain.Dtos;
using SpaceXMission_Service.Interfaces;
using SpaceXMission_Shared.Constants;
using SpaceXMission_Shared.Helpers.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace SpaceXMission_Service.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;

        public TokenService(IConfiguration configuration, IUserService userService)
        {
            _configuration = configuration;
            _userService = userService;
        }

        public JwtSecurityToken GetToken(IEnumerable<Claim> authClaims)
        {
            SymmetricSecurityKey authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
            int expiryInMinutes = int.TryParse(_configuration["JWT:ExpiryInMinutes"], out int hours) ? hours : 3;


            JwtSecurityToken token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddMinutes(expiryInMinutes),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256));

            return token;
        }
        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidAudience = _configuration["JWT:ValidAudience"],
                ValidIssuer = _configuration["JWT:ValidIssuer"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"])),
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;

            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");
            return principal;
        }

        public async Task<ApiResponse<AuthenticatedResponse>> Refresh(TokenModel tokenModel)
        {
            ApiResponse<AuthenticatedResponse> response = new();
            if (tokenModel is null)
            {
                response.Success = false;
                response.ErrorMessage = ErrorMessages.InvalidClientRequest;
                return response;
            }

            string accessToken = tokenModel.AccessToken;
            string refreshToken = tokenModel.RefreshToken;

            var principal = GetPrincipalFromExpiredToken(accessToken);
            var emailClaim = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
            var email = emailClaim?.Value;


            ApplicationUser user = await _userService.GetUserByUsernameAsync(email);

            if (user is null)
            {
                response.Success = false;
                response.ErrorMessage = ErrorMessages.UsernameDoesNotExist;
                return response;
            }

            if (user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
            {
                response.Success = false;
                response.ErrorMessage = ErrorMessages.InvalidClientRequest;
                return response;
            }

            var newAccessToken = GetToken(principal.Claims);
            var newRefreshToken = GenerateRefreshToken();

            await _userService.UpdateRefreshTokenAsync(user, newRefreshToken);


            response.Success = true;
            response.Data = new AuthenticatedResponse()
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
                RefreshToken = newRefreshToken,
                UserName = user.Email
            };

            return response;
        }
    }
}
